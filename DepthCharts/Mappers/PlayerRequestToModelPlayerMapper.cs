using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using DepthCharts.Models;
using Player = DepthCharts.Db.Player;

namespace DepthCharts.Mappers
{
    public class PlayerRequestToModelPlayerMapper : Profile
    {
        public PlayerRequestToModelPlayerMapper()
        {
            CreateMap<AddPlayerRequest, Models.Player>();
        }
    }
}
