using System;

namespace Web.AmazingLadies.Models
{
    public class LadyModel
    {
        public long ID { get; set; }
        public string Nickname { get; set; }
        public Discord Discord { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public OverwatchModel Overwatch { get; set; }
    }

    public class Discord
    {
        public string Name { get; set; }
        public int Tag { get; set; }
        public string FullName { get => Name + "#" + Tag; private set { } }
    }
}
