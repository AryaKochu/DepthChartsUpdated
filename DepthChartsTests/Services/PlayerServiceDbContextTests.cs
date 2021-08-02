using System;
using System.Collections.Generic;
using System.Text;
using DepthCharts.Db;
using Microsoft.EntityFrameworkCore;

namespace DepthChartsTests.Mock.Services
{
    public class PlayerServiceDbContextTests
    {
        protected DbContextOptions<PlayersDbContext> _contextOptions { get; }

        protected PlayerServiceDbContextTests(DbContextOptions<PlayersDbContext> contextOptions)
        {
            _contextOptions = contextOptions;
            Seed();
        }


        private void Seed()
        {
            using (var context = new PlayersDbContext(_contextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var bob = new Player
                {
                   Id =  1,
                   Name = "Bob", 
                   Position = "WR",
                   Depth = 9
                };

                var allice = new Player
                {
                    Id = 3,
                    Name = "Allice",
                    Position = "WR",
                    Depth = 8
                };

                var ryan = new Player
                {
                    Id = 2,
                    Name = "Ryan",
                    Position = "WR",
                    Depth = 7
                };

                context.AddRange(bob, allice, ryan);

                context.SaveChanges();
            }
        }
    }
}
