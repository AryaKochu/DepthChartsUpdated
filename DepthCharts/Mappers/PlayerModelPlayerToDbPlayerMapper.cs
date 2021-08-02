using AutoMapper;

namespace DepthCharts.Mappers
{
    public class PlayerModelPlayerToDbPlayerMapper : Profile
    {
        public PlayerModelPlayerToDbPlayerMapper()
        {
            CreateMap<Models.Player, Db.Player>();
        }
    }
}
