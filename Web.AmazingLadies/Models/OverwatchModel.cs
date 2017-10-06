using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AmazingLadies.Enums;

namespace Web.AmazingLadies.Models
{
    public class OverwatchModel
    {
        static string playOWurl = "https://playoverwatch.com/en-us/career/pc/{0}/{1}-{2}";

        public BattleTag BattleTag { get; set; }
        public Rank Rank { get; set; }
        public string FrameImage { get; set; }
        public string Avatar { get; set; }
        public ModesModel Modes { get; set; }
        public ServersEnum Server { get; set; }
        public RolesModel Roles { get; set; }
        public string Notes { get; set; }

        public string GetPlayOverwatchUrl() => string.Format(playOWurl, Server.ToString().ToLower(), BattleTag.Name, BattleTag.Tag);
    }

    public class BattleTag
    {
        public string Name { get; set; }
        public int Tag { get; set; }
        public string Full { get => Name + "#" + Tag; private set { } }
    }

}
