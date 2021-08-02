using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepthCharts.Models
{
    public class AddPlayerRequest
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public int Depth { get; set; }
    }
}
