using Newtonsoft.Json;

namespace Web.AmazingLadies.Helpers.APIs.SunDwarf
{
    public class RootObject
    {
        public Server EU { get; set; }
        public Server US { get; set; }
        public Server KR { get; set; }
    }

    public class Server
    {
        public Stats Stats { get; set; }
    }

    public class Stats
    {
        public Mode Competitive { get; set; }
    }

    public class Mode
    {
        [JsonProperty(PropertyName = "overall_stats")]
        public OverallStats OverallStats { get; set; }
    }
    
    public class OverallStats
    {
        public int Level { get; set; }
        [JsonProperty(PropertyName = "comprank")]
        public int SR { get; set; }
        [JsonProperty(PropertyName = "games")]
        public int NbGames { get; set; }
        [JsonProperty(PropertyName = "win_rate")]
        public float Winrate { get; set; }
        public int Losses { get; set; }
        [JsonProperty(PropertyName = "rank_image")]
        public string FrameLevel { get; set; }
        public int Wins { get; set; }
        public int Ties { get; set; }
        public int Prestige { get; set; }
        public string Avatar { get; set; }
        public string Tier { get; set; }
    }
}
