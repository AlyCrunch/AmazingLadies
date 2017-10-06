using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.AmazingLadies.Models
{
    public class Rank
    {
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
}