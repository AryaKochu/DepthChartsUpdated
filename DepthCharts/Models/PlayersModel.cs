using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepthCharts.Models
{

    public class PlayersModel
    {
        public IEnumerable<Player> Players { get; set; }
    }

    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int Depth { get; set; }
    }
}
