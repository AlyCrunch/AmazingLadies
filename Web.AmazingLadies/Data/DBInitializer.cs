using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AmazingLadies.Enums;
using Web.AmazingLadies.Models;

namespace Web.AmazingLadies.Data
{
    public class DbInitializer
    {
        public static void Initialize(ALOContext context)
        {
            context.Database.EnsureCreated();
            
            if (context.Ladies.Any())
            {
                return;   // DB has been seeded
            }

            var ladies = new LadyModel[]
            {
                new LadyModel()
                {
                    Nickname = "Crunch",
                    Discord = new Discord(){ Name = "Crunchy", Tag = 2554},
                    TimeZone = 33,
                    Overwatch = new OverwatchModel()
                    {
                        BattleTag = new BattleTag(){ Name = "Crunch", Tag = 2210 },
                        Rank = new Rank(){ SR = 2800 },
                        Server = ServersEnum.EU,
                        Modes = new Modes(){ Competitive = true, Arcade = true, Quick= true },
                        Roles = new Roles(){ DPS = false, Support = true, Tank = false },
                        Notes = "Loves playing Ana"
                    }
                },
                new LadyModel()
                {
                    Nickname = "QueenE",
                    Discord = new Discord(){ Name = "QueenE", Tag = 8106},
                    TimeZone = 30,
                    Overwatch = new OverwatchModel()
                    {
                        BattleTag = new BattleTag(){ Name = "QueenE", Tag = 2813 },
                        Rank = new Rank(){ SR = 3600 },
                        Server = ServersEnum.EU,
                        Modes = new Modes(){ Competitive = true, Arcade = false, Quick= true },
                        Roles = new Roles(){ DPS = true, Support = true, Tank = true },
                        Notes = "Loves Pizza"
                    }
                },
                new LadyModel()
                {
                    Nickname = "Alice",
                    Discord = new Discord(){ Name = "Alice", Tag = 6407},
                    TimeZone = 36,
                    Overwatch = new OverwatchModel()
                    {
                        BattleTag = new BattleTag(){ Name = "alice", Tag = 22732 },
                        Rank = new Rank(){ SR = 3800 },
                        Server = ServersEnum.KR,
                        Modes = new Modes(){ Competitive = true, Arcade = false, Quick= false },
                        Roles = new Roles(){ DPS = true, Support = false, Tank = false },
                        Notes = ""
                    }
                },
                new LadyModel()
                {
                    Nickname = "Noukky",
                    Discord = new Discord(){ Name = "Noukky", Tag = 3915},
                    TimeZone = 30,
                    Overwatch = new OverwatchModel()
                    {
                        BattleTag = new BattleTag(){ Name = "Noukky", Tag = 2688 },
                        Rank = new Rank(){ SR = 3600 },
                        Server = ServersEnum.US,
                        Modes = new Modes(){ Competitive = false, Arcade = false, Quick= true },
                        Roles = new Roles(){ DPS = false, Support = false, Tank = true },
                        Notes = ""
                    }
                }
            };

            foreach (LadyModel s in ladies)
            {
                context.Ladies.Add(s);
            }
            context.SaveChanges();
        }
    }
}
