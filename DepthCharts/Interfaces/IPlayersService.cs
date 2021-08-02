using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DepthCharts.Models;
using Player = DepthCharts.Db.Player;

namespace DepthCharts.Interfaces
{
    public interface IPlayersService
    {
        Task<bool> AddPlayerToDepthChart(AddPlayerRequest playerDetails);
        Task<List<FullDepthChartByPosition>> GetFullDepthChartByPosition(IList<Models.Player> savedPlayers = null);
        Task<bool> RemovePlayerFromDepthChart(string player, string position);
        Task<IList<int>> GetPlayersUnderPlayerInDepthChart(string player, string position);
    }
}
