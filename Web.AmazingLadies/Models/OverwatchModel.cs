using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.AmazingLadies.Enums;

namespace Web.AmazingLadies.Models
{
    public class OverwatchModel
    {
        public string BattleName { get; set; }
        public int BattleTag { get; set; }
        public int SR { get; set; }
        public ModesEnum[] Modes { get; set; }
        public ServersEnum Server { get; set; }
        public RolesModel Roles { get; set; }
        public string Notes { get; set; }

        public string GetBattleTag() => BattleName + "#" + BattleTag;
    }
}
