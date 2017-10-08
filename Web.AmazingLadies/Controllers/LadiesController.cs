using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.AmazingLadies.Models;
using Web.AmazingLadies.Helpers.APIs.SunDwarf;
using Web.AmazingLadies.Data;
using Microsoft.EntityFrameworkCore;
using Web.AmazingLadies.ViewModels;
using Web.AmazingLadies.Helpers;

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
            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "name";

            ViewData["NameSortParm"] = SortValueByKey("name", sortOrder);
            ViewData["SRSortParam"] = SortValueByKey("SR", sortOrder);
            ViewData["BattleTagSortParam"] = SortValueByKey("BT", sortOrder);

            if (filters.ContainsKey("sortOrder"))
            {
                filters.Remove("sortOrder");
                if (filters.Count > 0)
                    filters = Utilities.StringToDictionary(filters.First().Value);
            }

            ViewBag.Filters = Utilities.DictionaryToString(filters);


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

            var ladySearchViewModel = new LadiesViewModel(ladies, filters, sortOrder);

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

}