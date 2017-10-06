using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.AmazingLadies.Models;
using Web.AmazingLadies.Enums;
using Web.AmazingLadies.Helpers.APIs.SunDwarf;

namespace Web.AmazingLadies.Controllers
{
    public class LadiesController : Controller
    {
        OverwatchAPI OWAPI;

        public LadiesController()
        {
            OWAPI = new OverwatchAPI();
        }

        public async Task<IActionResult> Index()
        {
            var ladies = GetLadiesSample();

            //ladies = await UpdateLadies(ladies);

            ViewData["Ladies"] = ladies;
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }


        private List<LadyModel> GetLadiesSample()
        {
            return new List<LadyModel>()
            {
                new LadyModel()
                {
                    ID = 1,
                    Nickname = "Crunch",
                    Discord = new Discord(){ Name = "Crunchy", Tag = 2554},
                    TimeZone = TimeZoneInfo.Local,
                    Overwatch = new OverwatchModel()
                    {
                        BattleTag = new BattleTag(){ Name = "Crunch", Tag = 2210 },
                        Rank = new Rank(){ SR = 2800 },
                        Server = ServersEnum.EU,
                        Modes = new ModesModel(){ Competitive = true, Arcade = true, Quick= true },
                        Roles = new RolesModel(){ DPS = false, Support = true, Tank = false },
                        Notes = "Loves playing Ana"
                    }                    
                },
                new LadyModel()
                {
                    ID = 2,
                    Nickname = "QueenE",
                    Discord = new Discord(){ Name = "QueenE", Tag = 8106},
                    Overwatch = new OverwatchModel()
                    {
                        BattleTag = new BattleTag(){ Name = "QueenE", Tag = 2813 },
                        Rank = new Rank(){ SR = 3600 },
                        Server = ServersEnum.EU,
                        Modes = new ModesModel(){ Competitive = true, Arcade = false, Quick= true },
                        Roles = new RolesModel(){ DPS = true, Support = true, Tank = true },
                        Notes = "Loves Pizza"
                    }
                }
            };
        }

        private async Task<List<LadyModel>> UpdateLadies(List<LadyModel> ladies)
        {
            foreach (var lady in ladies)
            {
                var obj = await OWAPI.GetOverwatchProfile(lady.Overwatch.BattleTag.Name, lady.Overwatch.BattleTag.Tag);
                if (obj == null) return ladies;
                var stats = new OverallStats();

                switch(lady.Overwatch.Server.ToString())
                {
                    case "EU": stats = obj.EU.Stats.Competitive.OverallStats;break;
                    case "US": stats = obj.US.Stats.Competitive.OverallStats;break;
                    case "KR": stats = obj.KR.Stats.Competitive.OverallStats;break;
                }

                lady.Overwatch.Rank.SR = stats.SR;
                lady.Overwatch.Avatar = stats.Avatar;
                lady.Overwatch.FrameImage = stats.FrameLevel;
            }
            return ladies;
        }
    }
}