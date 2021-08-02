using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DepthCharts.Db;
using DepthCharts.Interfaces;
using DepthCharts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Player = DepthCharts.Db.Player;

namespace DepthCharts.Services
{
    public class PlayersService: IPlayersService
    {
        private PlayersDbContext _dbContext;
        private IMapper _mapper;

        public PlayersService(PlayersDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> AddPlayerToDepthChart(AddPlayerRequest details)
        {

            try
            {
                bool IsSuccess = false;
                var savedPlayers = await GetDataFromDb();
                if (savedPlayers == null)
                {
                    savedPlayers = new List<Models.Player>();
                    var newPlayer = _mapper.Map<AddPlayerRequest, Models.Player>(details);
                    savedPlayers.Add(newPlayer);
                    _dbContext.Players.AddRange(_mapper.Map<IEnumerable<Models.Player>, IEnumerable<Db.Player>>(savedPlayers));
                    _dbContext.SaveChanges();

                }
                else
                {
                    savedPlayers = InsertNewPlayer(savedPlayers, details);

                    var playerToDbEntity =
                        _mapper.Map<IEnumerable<Models.Player>, IEnumerable<Player>>(savedPlayers);

                    foreach (var player in playerToDbEntity)
                    {

                        var local = _dbContext.Set<Player>()
                            .Local
                            .FirstOrDefault(entry => entry.Id.Equals(player.Id));

                        // check if local is not null 
                        if (local != null)
                        {
                            // detach
                            _dbContext.Remove(local);
                        }
                        
                        _dbContext.Players.Add(player);
                        var result = _dbContext.SaveChanges();
                        IsSuccess = result == 1;
                    }
                }

                return IsSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> RemovePlayerFromDepthChart(string player, string position)
        {
            // check if local is not null 
            var local = _dbContext.Players.FirstOrDefault(l => l.Name == player);
            if (local != null)
            {
                // detach
                 _dbContext.Remove(local);
                _dbContext.SaveChanges();
            }

            return true;
        }

        public async Task<List<FullDepthChartByPosition>> GetFullDepthChartByPosition(IList<Models.Player> savedPlayers = null)
        {
            try
            {
                var playersByPos = new List<FullDepthChartByPosition>();
                savedPlayers = savedPlayers == null ? await GetDataFromDb() : savedPlayers;

                var players = savedPlayers?.OrderBy(p => p.Depth).GroupBy(i => i.Position).ToList();
                foreach (var result in players)
                {
                   playersByPos.Add( new FullDepthChartByPosition
                   {
                       Position = result.Key,
                       Id = result.Select(_ => _.Id).ToList()
                   });
                }

                return playersByPos;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IList<int>> GetPlayersUnderPlayerInDepthChart(string player, string position)
        {
            var savedPlayers = await GetDataFromDb();
            var playersUnderGivenPos = savedPlayers.Where(p => p.Position == position).ToList();
            var depthOfGivenPlayer = playersUnderGivenPos.Where(p => p.Name == player).ToList();

           return playersUnderGivenPos.Where(t => depthOfGivenPlayer.FirstOrDefault()?.Depth < t.Depth).Select(i => i.Id).ToList();
        }

        private async Task<IList<Models.Player>> GetDataFromDb()
        {
            var players = await _dbContext.Players.ToListAsync();
            if (players != null && players.Any())
            {
                var result = _mapper.Map<IEnumerable<Db.Player>, IEnumerable<Models.Player>>(players).ToList();
                return result;
            }

            return null;
        }

        private IList<Models.Player> InsertNewPlayer(IList<Models.Player> savedPlayers, AddPlayerRequest details)
        {
            var newPlayer = _mapper.Map<AddPlayerRequest, Models.Player>(details);
            var playersInPosition = savedPlayers.Where(p => p.Position == details.Position).ToList();

            if (playersInPosition.Any())
            {
                var sortedByDepth = playersInPosition.OrderBy(o => o.Depth).ToList();
                // playersInPosition.Insert(details.Depth, newPlayer);
                // playersInPosition.Add(newPlayer);
                for (int i = 0; i <= sortedByDepth.Count - 1; i++)
                {
                    if (newPlayer.Depth == sortedByDepth[i].Depth)
                    {
                        for (int j = i; j < sortedByDepth.Count;j++)
                        {
                            sortedByDepth[j].Depth++;
                        }
                    }
                }
                sortedByDepth.Add(newPlayer);
                return sortedByDepth;
                /*for (int i = 0; i < playersInPosition.Count; i++)
                {
                    playersInPosition[i].Depth = i;
                }*/
            }
            else
            {
                playersInPosition.Add(newPlayer);
            }

            return playersInPosition;
        }

    }
}
