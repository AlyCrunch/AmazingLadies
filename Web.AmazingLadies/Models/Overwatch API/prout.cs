using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.AmazingLadies.Models.Overwatch_API
{
    public class Rootobject
    {
        public object Any { get; set; }
        public Modes EU { get; set; }
        public Modes US { get; set; }
        public Modes KR { get; set; }
        public _Request _request { get; set; }
    }

    public class Modes
    {
        public Mode Competitive { get; set; }
        public Mode Quickplay { get; set; }
    }

    public class Mode
    {
        public Average_Stats Average_stats { get; set; }
        public bool competitive { get; set; }
        public Overall_Stats overall_stats { get; set; }
        public Rolling_Average_Stats rolling_average_stats { get; set; }
        public Game_Stats game_stats { get; set; }
    }

    public class Average_Stats
    {
    }

    public class Overall_Stats
    {
        public int level { get; set; }
        public int comprank { get; set; }
        public int games { get; set; }
        public float win_rate { get; set; }
        public int losses { get; set; }
        public string rank_image { get; set; }
        public int wins { get; set; }
        public int ties { get; set; }
        public int prestige { get; set; }
        public string avatar { get; set; }
        public string tier { get; set; }
    }

    public class Rolling_Average_Stats
    {
        public float Solo_kills { get; set; }
        public float objective_time { get; set; }
        public float eliminations { get; set; }
        public float healing_done { get; set; }
        public float deaths { get; set; }
        public float all_damage_done { get; set; }
        public float objective_kills { get; set; }
        public float time_spent_on_fire { get; set; }
        public float final_blows { get; set; }
    }

    public class Game_Stats
    {
        public float turret_destroyed_most_in_game { get; set; }
        public float turret_destroyed { get; set; }
        public float all_damage_done { get; set; }
        public float final_blows { get; set; }
        public float deaths { get; set; }
        public float games_lost { get; set; }
        public float environmental_kill_most_in_game { get; set; }
        public float games_played { get; set; }
        public float solo_kills { get; set; }
        public float medals_silver { get; set; }
        public float objective_time { get; set; }
        public float healing_done_most_in_game { get; set; }
        public float healing_done { get; set; }
        public float kill_streak_best { get; set; }
        public float eliminations_most_in_game { get; set; }
        public float objective_kills { get; set; }
        public float games_won { get; set; }
        public float defensive_assists_most_in_game { get; set; }
        public float kpd { get; set; }
        public float objective_kills_most_in_game { get; set; }
        public float final_blows_most_in_game { get; set; }
        public float melee_final_blows_most_in_game { get; set; }
        public float medals_gold { get; set; }
        public float cards { get; set; }
        public float medals { get; set; }
        public float time_spent_on_fire_most_in_game { get; set; }
        public float eliminations { get; set; }
        public float defensive_assists { get; set; }
        public float time_played { get; set; }
        public float offensive_assists_most_in_game { get; set; }
        public float medals_bronze { get; set; }
        public float objective_time_most_in_game { get; set; }
        public float melee_final_blows { get; set; }
        public float solo_kills_most_in_game { get; set; }
        public float all_damage_done_most_in_game { get; set; }
        public float environmental_kills { get; set; }
        public float offensive_assists { get; set; }
        public float time_spent_on_fire { get; set; }
    }
        
    public class _Request
    {
        public int api_ver { get; set; }
        public string route { get; set; }
    }

}
