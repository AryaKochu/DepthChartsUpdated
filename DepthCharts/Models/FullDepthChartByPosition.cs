using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepthCharts.Models
{
    public class FullDepthChartByPosition
    {
        public string Position { get; set; }
        public IList<int> Id { get; set; }
    }
}
