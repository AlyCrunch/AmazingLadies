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

        public IActionResult Index(string sortOrder, string serverString, string roleString, string modeString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SRSortParam"] = sortOrder == "SR" ? "SR_desc" : "SR";
            ViewData["servFilter"] = serverString;
            ViewData["roleFilter"] = roleString;
            ViewData["modeFilter"] = modeString;
            
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

            if (!String.IsNullOrEmpty(serverString))
            {
                ladies = ladies.Where(s => serverString.ToUpper().Contains(s.Overwatch.Server.ToString().ToUpper())).ToList();
            }

            if (!String.IsNullOrEmpty(roleString))
            {
                ladies = ladies.Where(s => s.Overwatch.Roles.HasAtLeastOneRole(roleString.ToUpper())).ToList();
            }

            if (!String.IsNullOrEmpty(modeString))
            {
                ladies = ladies.Where(s => s.Overwatch.Modes.HasAtLeastOneMode(modeString.ToUpper())).ToList();
            }

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