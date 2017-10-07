using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.AmazingLadies.Models
{
    public class LadyModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string Nickname { get; set; }
        public Discord Discord { get; set; }
        public int TimeZone { get; set; }
        public OverwatchModel Overwatch { get; set; }
    }

    public class Discord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Tag { get; set; }
        public string FullName { get => Name + "#" + Tag; private set { } }
    }
}
