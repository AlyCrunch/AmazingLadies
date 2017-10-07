using Web.AmazingLadies.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.AmazingLadies.Models
{
    public class OverwatchModel
    {
        private string playOWurl = "https://playoverwatch.com/en-us/career/pc/{0}/{1}-{2}";

        public OverwatchModel()
        {
            Avatar = string.Empty;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public BattleTag BattleTag { get; set; }
        public Rank Rank { get; set; }
        public string FrameImage { get; set; }
        public string Avatar { get; set; }
        public Modes Modes { get; set; }
        public ServersEnum Server { get; set; }
        public Roles Roles { get; set; }
        public string Notes { get; set; }

        public string GetPlayOverwatchUrl() => string.Format(playOWurl, Server.ToString().ToLower(), BattleTag.Name, BattleTag.Tag);
    }

    public class BattleTag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Tag { get; set; }
        public string Full { get => Name + "#" + Tag; private set { } }
    }

    public class Rank
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int SR { get; set; }
        public string Name
        {
            get
            {
                if (SR < 1500)
                    return "Bronze";
                if (SR < 2000)
                    return "Silver";
                if (SR < 2500)
                    return "Gold";
                if (SR < 3000)
                    return "Platinum";
                if (SR < 3500)
                    return "Diamond";
                if (SR < 4000)
                    return "Master";
                return "Grandmaster";
            }
        }
        public string Image
        {
            get
            {
                string path = "~/images/Ranks/rank-{0}.png";

                if (SR < 1500)
                    return string.Format(path, 1);
                if (SR < 2000)
                    return string.Format(path, 2);
                if (SR < 2500)
                    return string.Format(path, 3);
                if (SR < 3000)
                    return string.Format(path, 4);
                if (SR < 3500)
                    return string.Format(path, 5);
                if (SR < 4000)
                    return string.Format(path, 6);
                return string.Format(path, 7);
            }
        }
    }

    public class Roles
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public bool Support { get; set; }
        public bool DPS { get; set; }
        public bool Tank { get; set; }

        public bool HasAtLeastOneRole(string RoleList)
        {
            //DPS-TANK-SUPPORT
            if (RoleList.Contains("DPS") && DPS)
                return true;
            if (RoleList.Contains("TANK") && Tank)
                return true;
            if (RoleList.Contains("SUPPORT") && Support)
                return true;
            return false;
        }
    }

    public class Modes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public bool Competitive { get; set; }
        public bool Quick { get; set; }
        public bool Arcade { get; set; }

        public bool HasAtLeastOneMode(string ModeList)
        {
            //COMP-QUICK-ARCADE
            if (ModeList.Contains("COMP") && Competitive)
                return true;
            if (ModeList.Contains("QUICK") && Quick)
                return true;
            if (ModeList.Contains("ARCADE") && Arcade)
                return true;
            return false;
        }
    }

}
