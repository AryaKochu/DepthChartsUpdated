using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DepthCharts.Db;
using DepthCharts.Interfaces;
using DepthCharts.Mappers;
using DepthCharts.Models;
using DepthCharts.Services;
using DepthChartsTests.Mock.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;
using FluentAssertions;
using Player = DepthCharts.Db.Player;

namespace DepthChartsTests
{
    public class PlayerServiceTests : PlayerServiceDbContextTests
    {
        private PlayersDbContext _dbContext;
        private IPlayersService _service;
        private IMapper _mapper;

        public PlayerServiceTests() : base(
        new DbContextOptionsBuilder<PlayersDbContext>()
            .UseInMemoryDatabase("PlayersTest")
            .Options)
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PlayerRequestToModelPlayerMapper());
                cfg.AddProfile(new PlayerModelPlayerToDbPlayerMapper());
                cfg.AddProfile(new PlayerRequestToDbPlayerMapper());
                cfg.AddProfile(new PlayerDbToPlayerModelMapper());
            });
            _mapper = mockMapper.CreateMapper();
        }
        [Fact]
        public async Task RetrievePlayersByPosition_ReturnsSuccess()
        {
            //Arrange
            var savedPlayers = JsonConvert.DeserializeObject<IEnumerable<Player>>(await File.ReadAllTextAsync("Mock/SavedPlayersList.json"));
            
            using (var context = new PlayersDbContext(_contextOptions))
            {
                _service = Substitute.For<PlayersService>(context, _mapper);

                //Act
                var result = await _service.GetFullDepthChartByPosition();

                //Assert
                Assert.Equal(1, result.Count); // groupedby position
                Assert.Equal(2, result[0].Id[0]);
                Assert.Equal(3, result[0].Id[1]);
                Assert.Equal(1, result[0].Id[2]);
            }
        }

        [Fact]
        public async Task AddPlayersByPosition_ReturnsSuccess()
        {
            //Arrange
            var newPlayer = new AddPlayerRequest
            {
                Depth = 2,
                Position = "WR",
                Name = "Ben"
            };

            using (var context = new PlayersDbContext(_contextOptions))
            {
                _service = Substitute.For<PlayersService>(context, _mapper);

                //Act
                var result = await _service.AddPlayerToDepthChart(newPlayer);

                //Assert
                Assert.Equal(true, result);
            }
        }

        [Fact]
        public async Task RemovePlayer_ReturnsSuccess()
        {
            //Arrange
            var removePlayer = new AddPlayerRequest
            {
                Position = "WR",
                Name = "Ryan"
            };

            using (var context = new PlayersDbContext(_contextOptions))
            {
                _service = Substitute.For<PlayersService>(context, _mapper);

                //Act
                var result = await _service.RemovePlayerFromDepthChart(removePlayer.Name, removePlayer.Position);

                //Assert
                var data = context.Players.ToListAsync().Result;
                Assert.Equal(true, result);
            }
        }
    }
}
