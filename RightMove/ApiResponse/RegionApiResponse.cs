using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMove.ApiResponse
{
    public class RegionApiResponse
    {
        public Match[] matches { get; set; }
    }

    public class Match
    {
        public string id { get; set; }
        public string type { get; set; }
        public string displayName { get; set; }
        public string highlighting { get; set; }
        public Highlight[] highlights { get; set; }
    }

    public class Highlight
    {
        public string text { get; set; }
        public bool highlighted { get; set; }
    }

}
