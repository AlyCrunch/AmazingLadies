using System.Collections.Generic;
using System.Linq;
using Web.AmazingLadies.Enums;
using Web.AmazingLadies.Models;

namespace Web.AmazingLadies.ViewModels
{
    public class LadiesViewModel
    {
        public List<LadyModel> Ladies { get; set; }
        public LadiesFilter Filter { get; set; }
        public LadiesSorting Sorting { get; set; }

        public LadiesViewModel(List<LadyModel> ladies, Dictionary<string, string> filters, string sort)
        {
            Ladies = ladies;
            Filter = new LadiesFilter(filters);
            Sorting = new LadiesSorting(sort);
        }

        public bool FilteringLadies()
        {
            try
            {
                if (!Filter.HasFilter) return true;

                if (Filter.Servers.Count > 0 && Filter.Servers.Count < 3)
                    Ladies = Ladies.Where(s => Filter.Servers.Contains(s.Overwatch.Server)).ToList();

                var roleList = string.Join('-', Filter.Roles);
                if (Filter.Roles.Count > 0 && Filter.Roles.Count < 3)
                    Ladies = Ladies.Where(r => r.Overwatch.Roles.HasAtLeastOneRole(Filter.Roles)).ToList();

                var modeList = string.Join('-', Filter.Modes);
                if (Filter.Modes.Count > 0 && Filter.Modes.Count < 3)
                    Ladies = Ladies.Where(r => r.Overwatch.Modes.HasAtLeastOneMode(Filter.Modes)).ToList();
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

    public class LadiesFilter
    {
        public bool HasFilter { get; set; }
        public List<ServersEnum> Servers { get; set; }
        public List<RolesEnum> Roles { get; set; }
        public List<ModesEnum> Modes { get; set; }
        public string HasFilterStyle
        {
            get
            {
                if (HasFilter)
                    return "show";
                return "";
            }
        }
        public ServersStyle ServersStyle { get; set; }
        public RolesStyle RolesStyle { get; set; }
        public ModesStyle ModesStyle { get; set; }

        public LadiesFilter()
        {
            HasFilter = false;
            Servers = new List<ServersEnum>();
            Roles = new List<RolesEnum>();
            Modes = new List<ModesEnum>();
        }

        public LadiesFilter(Dictionary<string, string> filters)
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

            ServersStyle = new ServersStyle(Servers);
            RolesStyle = new RolesStyle(Roles);
            ModesStyle = new ModesStyle(Modes);
        }
    }

    public class LadiesSorting
    {
        public string SortProperty { get; set; }
        private string _nickname = "Nickname";
        public string NicknameText
        {
            get
            {
                if (SortProperty.Contains("name"))
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

        public LadiesSorting(string sort)
        {
            SortProperty = sort;
        }
    }

    public class ServersStyle
    {
        public ServersStyle(List<ServersEnum> list)
        {
            US = new Style(list.Contains(ServersEnum.US));
            EU = new Style(list.Contains(ServersEnum.EU));
            KR = new Style(list.Contains(ServersEnum.KR));
        }

        public Style US { get; set; }
        public Style EU { get; set; }
        public Style KR { get; set; }
    }

    public class RolesStyle
    {
        public RolesStyle(List<RolesEnum> list)
        {
            DPS = new Style(list.Contains(RolesEnum.DPS));
            Tank = new Style(list.Contains(RolesEnum.Tank));
            Support = new Style(list.Contains(RolesEnum.Support));
        }

        public Style DPS { get; set; }
        public Style Tank { get; set; }
        public Style Support { get; set; }
    }

    public class ModesStyle
    {
        public ModesStyle(List<ModesEnum> list)
        {
            Competitive = new Style(list.Contains(ModesEnum.Competitive));
            Quick = new Style(list.Contains(ModesEnum.Quick));
            Arcade = new Style(list.Contains(ModesEnum.Arcade));
        }

        public Style Competitive { get; set; }
        public Style Quick { get; set; }
        public Style Arcade { get; set; }
    }

    public class Style
    {
        public Style(bool active)
        {
            if (active)
            {
                Class = "active";
                Input = "checked";
            }
            else
            {
                Class = "";
                Input = "";
            }
        }
        private string _class;
        public string Class
        {
            get { return _class; }
            set { _class = value; }
        }

        private string _input;
        public string Input
        {
            get { return _input; }
            set { _input = value; }
        }

    }
}