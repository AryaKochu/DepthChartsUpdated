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
    public class PlayerRequestToDbPlayerMapper : Profile
    {
        public PlayerRequestToDbPlayerMapper()
        {
            CreateMap<AddPlayerRequest, Player>();
        }
    }
}
