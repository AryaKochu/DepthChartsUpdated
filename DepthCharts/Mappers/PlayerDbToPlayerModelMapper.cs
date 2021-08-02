using AutoMapper;

namespace DepthCharts.Mappers
{
    public class PlayerDbToPlayerModelMapper : Profile
    {
        public PlayerDbToPlayerModelMapper()
        {
            CreateMap<Db.Player, Models.Player>();
        }
    }
}
