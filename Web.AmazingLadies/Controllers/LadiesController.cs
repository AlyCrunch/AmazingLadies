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
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SRSortParam"] = sortOrder == "SR" ? "SR_desc" : "SR";

            Filters(filters);

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

            ladies = FilteringLadies(filters, ladies);

            switch (sortOrder)
            {
                case "name_desc":
                    ladies = ladies.OrderByDescending(s => s.Overwatch.Rank.SR).ToList();
                    break;
                case "SR":
                    ladies = ladies.OrderBy(s => s.Overwatch.Rank.SR).ToList();
                    break;
                case "SR_desc":
                    ladies = ladies.OrderByDescending(s => s.Nickname).ToList();
                    break;
                default:
                    ladies = ladies.OrderBy(s => s.Nickname).ToList();
                    break;
            }

            return View(ladies);
        }

        public IActionResult Create()
        {
            return View();
        }

        private List<LadyModel> FilteringLadies(Dictionary<string, string> filters, List<LadyModel> ladies)
        {
            if (filters.Count == 0) return ladies;

            var serverList = new List<string>();
            if (filters.ContainsKey("s-u")) serverList.Add("US");
            if (filters.ContainsKey("s-e")) serverList.Add("EU");
            if (filters.ContainsKey("s-k")) serverList.Add("KR");
            if (serverList.Count > 0 && serverList.Count < 3)
                ladies = ladies.Where(s => serverList.Contains(s.Overwatch.Server.ToString())).ToList();

            var roleList = new List<string>();
            if (filters.ContainsKey("r-d")) roleList.Add("DPS");
            if (filters.ContainsKey("r-t")) roleList.Add("TANK");
            if (filters.ContainsKey("r-s")) roleList.Add("SUPPORT");
            if (roleList.Count > 0 && roleList.Count < 3)
                ladies = ladies.Where(r => r.Overwatch.Roles.HasAtLeastOneRole(string.Join('-', roleList))).ToList();

            var modeList = new List<string>();
            if (filters.ContainsKey("m-c")) modeList.Add("COMP");
            if (filters.ContainsKey("m-q")) modeList.Add("QUICK");
            if (filters.ContainsKey("m-a")) modeList.Add("ARCADE");
            if (modeList.Count > 0 && modeList.Count < 3)
                ladies = ladies.Where(m => m.Overwatch.Modes.HasAtLeastOneMode(string.Join('-', modeList))).ToList();

            return ladies;
        }

        private void Filters(Dictionary<string, string> filters)
        {
            ViewData["s-u"] = filters.ContainsKey("s-u") ? "checked" : "";
            ViewData["s-u_c"] = filters.ContainsKey("s-u") ? "active" : "";
            ViewData["s-e"] = filters.ContainsKey("s-e") ? "checked" : "";
            ViewData["s-e_c"] = filters.ContainsKey("s-e") ? "active" : "";
            ViewData["s-k"] = filters.ContainsKey("s-k") ? "checked" : "";
            ViewData["s-k_c"] = filters.ContainsKey("s-k") ? "active" : "";

            ViewData["r-d"] = filters.ContainsKey("r-d") ? "checked" : "";
            ViewData["r-d_c"] = filters.ContainsKey("r-d") ? "active" : "";
            ViewData["r-t"] = filters.ContainsKey("r-t") ? "checked" : "";
            ViewData["r-t_c"] = filters.ContainsKey("r-t") ? "active" : "";
            ViewData["r-s"] = filters.ContainsKey("r-s") ? "checked" : "";
            ViewData["r-s_c"] = filters.ContainsKey("r-s") ? "active" : "";

            ViewData["m-c"] = filters.ContainsKey("m-c") ? "checked" : "";
            ViewData["m-c_c"] = filters.ContainsKey("m-c") ? "active" : "";
            ViewData["m-q"] = filters.ContainsKey("m-q") ? "checked" : "";
            ViewData["m-q_c"] = filters.ContainsKey("m-q") ? "active" : "";
            ViewData["m-a"] = filters.ContainsKey("m-a") ? "checked" : "";
            ViewData["m-a_c"] = filters.ContainsKey("m-a") ? "active" : "";

            ViewData["HasFilter"] = (filters.Count > 0) ? "show" : "";
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
}