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

        private List<LadyModel> GetLadiesSample()
        {
            return new List<LadyModel>()
            {
                new LadyModel()
                {
                    ID = 1,
                    Nickname = "Crunchy",
                    DiscordAccount = "Crunchy#2554",
                    Overwatch = new OverwatchModel()
                    {
                        BattleName = "Crunch",
                        BattleTag = 2210,
                        SR = 2801,
                        Server = ServersEnum.EU,
                        Modes = new ModesEnum[] { ModesEnum.Arcade, ModesEnum.Competitive, ModesEnum.Quick },
                        Roles = new RolesModel(){ DPS = false, Support = true, Tank = false },
                        Notes = "Loves playing Ana"
                    }
                },
                new LadyModel()
                {
                    ID = 2,
                    Nickname = "QueenE",
                    DiscordAccount = "QueenE#8106",
                    Overwatch = new OverwatchModel()
                    {
                        BattleName = "QueenE",
                        BattleTag = 2813 ,
                        SR = 3600,
                        Server = ServersEnum.EU,
                        Modes = new ModesEnum[] { ModesEnum.Competitive, ModesEnum.Quick },
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
                var obj = await OWAPI.GetOverwatchProfile(lady.Overwatch.BattleName, lady.Overwatch.BattleTag);
                if (obj == null) return ladies;

                switch(lady.Overwatch.Server.ToString())
                {
                    case "EU": lady.Overwatch.SR = obj.EU.Stats.Competitive.OverallStats.SR;break;
                    case "NA": lady.Overwatch.SR = obj.US.Stats.Competitive.OverallStats.SR;break;
                    case "KR": lady.Overwatch.SR = obj.KR.Stats.Competitive.OverallStats.SR;break;
                }
            }
            return ladies;
        }
    }
}