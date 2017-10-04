using System;

namespace Web.AmazingLadies.Models
{
    public class LadyModel
    {
        public long ID { get; set; }
        public string Nickname { get; set; }
        public string DiscordAccount { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public OverwatchModel Overwatch { get; set; }
    }
}
