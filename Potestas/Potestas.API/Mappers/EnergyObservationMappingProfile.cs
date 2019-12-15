using AutoMapper;
using Potestas.API.ViewModels;
using Potestas.ORM.Plugin.Models;

namespace Potestas.API.Mappers
{
    public class EnergyObservationMappingProfile : Profile
    {
        public EnergyObservationMappingProfile()
        {
            CreateMap<CoordinatesModel, EnergyObservations>();

            CreateMap<CoordinatesModel, ORM.Plugin.Models.Coordinates>().ReverseMap();

            CreateMap<CoordinatesModel, Coordinates>()
                .ConstructUsing(src => new Coordinates(src.Id, src.X, src.Y));

            CreateMap<Coordinates, CoordinatesModel>()
                .ConstructUsing(src => new CoordinatesModel { Id = src.Id, X = src.X, Y = src.Y });

            CreateMap<EnergyObservations, EnergyObservationModel>()
                    .ForMember(d => d.ObservationPoint, opt => opt.MapFrom(src => src.Coordinate))
                    .ReverseMap()
                    .ForPath(d => d.Coordinate, opt => opt.MapFrom(src => src.ObservationPoint));

            CreateMap<EnergyObservationModel, IEnergyObservation>()
                .ConstructUsing(src => new EnergyObservations()
                {
                    Id = src.Id,
                    Coordinate = new ORM.Plugin.Models.Coordinates() { Id = src.ObservationPoint.Id, X = src.ObservationPoint.X, Y = src.ObservationPoint.Y },
                    EstimatedValue = src.EstimatedValue,
                    ObservationTime = src.ObservationTime
                });
        }
    }

    //https://stackoverflow.com/questions/40275195/how-to-set-up-automapper-in-asp-net-core
    //https://lostechies.com/jimmybogard/2009/04/15/automapper-feature-interfaces-and-dynamic-mapping/
    //http://docs.automapper.org/en/stable/index.html
}
