using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.AmazingLadies.Models;
using Web.AmazingLadies.Enums;
using Web.AmazingLadies.Helpers.APIs.SunDwarf;
using Web.AmazingLadies.Data;
using Microsoft.EntityFrameworkCore;

namespace Web.AmazingLadies.Controllers
{
    public class LadiesController : Controller
    {
        OverwatchAPI OWAPI;
        private readonly ALOContext _context;

        public LadiesController(ALOContext context)
        {
            _context = context;
            OWAPI = new OverwatchAPI();
        }

        public IActionResult Index(string sortOrder, Dictionary<string, string> filters)
        {
            ViewData["NameSortParm"] = SortValueByKey("name", sortOrder);
            ViewData["SRSortParam"] = SortValueByKey("SR", sortOrder);
            ViewData["BattleTagSortParam"] = SortValueByKey("BT", sortOrder);
            if (filters.ContainsKey("sortOrder")) filters.Remove("sortOrder");

            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "name";

            var ladies = _context.Ladies
                                 .Include(l => l.Overwatch)
                                    .ThenInclude(o => o.BattleTag)
                                 .Include(l => l.Overwatch)
                                    .ThenInclude(o => o.Modes)
                                 .Include(l => l.Overwatch)
                                    .ThenInclude(o => o.Rank)
                                 .Include(l => l.Overwatch)
                                    .ThenInclude(o => o.Roles)
                                 .ToList();

            var ladySearchViewModel = new LadySearchViewModel(ladies, filters, sortOrder);

            ladySearchViewModel.FilteringLadies();
            ladySearchViewModel.SortingLadies();

            return View(ladySearchViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        private string SortValueByKey(string key, string sort)
        {
            return sort == key ? key + "_desc" : key;
        }

        private async Task<List<LadyModel>> UpdateLadies(List<LadyModel> ladies)
        {
            foreach (var lady in ladies)
            {
                var obj = await OWAPI.GetOverwatchProfile(lady.Overwatch.BattleTag.Name, lady.Overwatch.BattleTag.Tag);
                if (obj == null) return ladies;
                var stats = new OverallStats();

                switch (lady.Overwatch.Server.ToString())
                {
                    case "EU": stats = obj.EU.Stats.Competitive.OverallStats; break;
                    case "US": stats = obj.US.Stats.Competitive.OverallStats; break;
                    case "KR": stats = obj.KR.Stats.Competitive.OverallStats; break;
                }

                lady.Overwatch.Rank.SR = stats.SR;
                lady.Overwatch.Avatar = stats.Avatar;
                lady.Overwatch.FrameImage = stats.FrameLevel;
            }
            return ladies;
        }
    }

    public class LadySearchViewModel
    {
        public List<LadyModel> Ladies { get; set; }
        public LadyFilter Filter { get; set; }
        public LadySorting Sorting { get; set; }

        public LadySearchViewModel(List<LadyModel> ladies, Dictionary<string, string> filters, string sort)
        {
            Ladies = ladies;
            Filter = new LadyFilter(filters);
            Sorting = new LadySorting(sort);
        }

        public bool FilteringLadies()
        {
            try
            {
                if (!Filter.HasFilter) return true;

                var serverList = string.Join('-', Filter.Servers);
                if (Filter.Servers.Count > 0 && Filter.Servers.Count < 3)
                    Ladies = Ladies.Where(s => serverList.Contains(s.Overwatch.Server.ToString())).ToList();

                var roleList = string.Join('-', Filter.Roles);
                if (Filter.Roles.Count > 0 && Filter.Roles.Count < 3)
                    Ladies = Ladies.Where(r => r.Overwatch.Roles.HasAtLeastOneRole(string.Join('-', roleList))).ToList();

                var modeList = string.Join('-', Filter.Modes);
                if (Filter.Modes.Count > 0 && Filter.Modes.Count < 3)
                    Ladies = Ladies.Where(r => r.Overwatch.Modes.HasAtLeastOneMode(string.Join('-', modeList))).ToList();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool SortingLadies()
        {
            try
            {
                switch (Sorting.SortProperty)
                {
                    case "SR_desc":
                        Ladies = Ladies.OrderByDescending(s => s.Overwatch.Rank.SR).ToList();
                        break;
                    case "SR":
                        Ladies = Ladies.OrderBy(s => s.Overwatch.Rank.SR).ToList();
                        break;
                    case "BT_desc":
                        Ladies = Ladies.OrderByDescending(s => s.Overwatch.BattleTag.Name).ToList();
                        break;
                    case "BT":
                        Ladies = Ladies.OrderBy(s => s.Overwatch.BattleTag.Name).ToList();
                        break;
                    case "name_desc":
                        Ladies = Ladies.OrderByDescending(s => s.Nickname).ToList();
                        break;
                    default:
                        Ladies = Ladies.OrderBy(s => s.Nickname).ToList();
                        break;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

    public class LadyFilter
    {
        public bool HasFilter { get; set; }
        public List<ServersEnum> Servers { get; set; }
        public List<RolesEnum> Roles { get; set; }
        public List<ModesEnum> Modes { get; set; }

        public LadyFilter()
        {
            HasFilter = false;
            Servers = new List<ServersEnum>();
            Roles = new List<RolesEnum>();
            Modes = new List<ModesEnum>();
        }
        public LadyFilter(Dictionary<string, string> filters)
        {
            HasFilter = false;
            Servers = new List<ServersEnum>();
            Roles = new List<RolesEnum>();
            Modes = new List<ModesEnum>();

            if (filters.ContainsKey("s-e"))
                Servers.Add(ServersEnum.EU);
            if (filters.ContainsKey("s-k"))
                Servers.Add(ServersEnum.KR);
            if (filters.ContainsKey("s-u"))
                Servers.Add(ServersEnum.US);


            if (filters.ContainsKey("r-d"))
                Roles.Add(RolesEnum.DPS);
            if (filters.ContainsKey("r-t"))
                Roles.Add(RolesEnum.Tank);
            if (filters.ContainsKey("r-s"))
                Roles.Add(RolesEnum.Support);


            if (filters.ContainsKey("m-c"))
                Modes.Add(ModesEnum.Competitive);
            if (filters.ContainsKey("m-q"))
                Modes.Add(ModesEnum.Quick);
            if (filters.ContainsKey("m-a"))
                Modes.Add(ModesEnum.Arcade);

            HasFilter = filters.Count > 0;
        }

    }
    public class LadySorting
    {
        public string SortProperty { get; set; }
        private string _nickname = "Nickname";
        public string NicknameText
        {
            get
            {
                if(SortProperty.Contains("name"))
                {
                    if (SortProperty.Contains("_desc"))
                        return $"{_nickname} ▾";
                    else
                        return $"{_nickname} ▴";
                }
                return _nickname;
            }
        }
        private string _BT = "BattleTag";
        public string BTText
        {
            get
            {
                if (SortProperty.Contains("BT"))
                {
                    if (SortProperty.Contains("_desc"))
                        return $"{_BT} ▾";
                    else
                        return $"{_BT} ▴";
                }
                return _BT;
            }
        }
        private string _SR = "Season Rating";
        public string SRText
        {
            get
            {
                if (SortProperty.Contains("SR"))
                {
                    if (SortProperty.Contains("_desc"))
                        return $"{_SR} ▾";
                    else
                        return $"{_SR} ▴";
                }
                return _SR;
            }
        }

        public LadySorting(string sort)
        {
            SortProperty = sort;
        }
    }
}