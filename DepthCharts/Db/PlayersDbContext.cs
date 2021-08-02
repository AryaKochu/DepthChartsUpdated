using DepthCharts.Models;
using Microsoft.EntityFrameworkCore;

namespace DepthCharts.Db
{
    public class PlayersDbContext: DbContext
    {
        public PlayersDbContext(DbContextOptions options): base(options)
        {
            
        }
        public DbSet<Player> Players { get; set; }
    }
}
