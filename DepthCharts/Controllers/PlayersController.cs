using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using DepthCharts.Interfaces;
using DepthCharts.Models;
using Microsoft.Extensions.Logging;

namespace DepthCharts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : Controller
    {
        private ILogger<PlayersController> _logger;
        private IPlayersService _service;

        public PlayersController(ILogger<PlayersController> logger, IPlayersService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("getDepthChart")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetDepthChart()
        {
            return Ok(await _service.GetFullDepthChartByPosition());
        }

        [HttpPost("addPlayer")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AddPlayer([FromBody] AddPlayerRequest playerDetails) 
        {
            return Ok(await _service.AddPlayerToDepthChart(playerDetails));
        }

        [HttpGet("removePlayer")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RemovePlayer(string player, string position)
        {
            return Ok(await _service.RemovePlayerFromDepthChart(player, position));
        }
        
        [HttpGet("getPlayerByPosition")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetPlayersUnderPlayerInDepthChart(string player, string position)
        {
            return Ok(await _service.GetPlayersUnderPlayerInDepthChart(player, position));
        }
    }
}
