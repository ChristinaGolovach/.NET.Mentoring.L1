using AutoMapper;
using Potestas.API.ViewModels;

namespace Potestas.API.Mappers
{
    public class EnergyObservationMappingProfile : Profile
    {
        public EnergyObservationMappingProfile()
        {
            //CreateMap<EnergyObservationModel, IEnergyObservation>()
            //    .ForMember(d => d.ObservationPoint, opt => opt.MapFrom(src => src.ObservationPoint))
            //    .ReverseMap()
            //    .ForPath(s => s.ObservationPoint, opt => opt.MapFrom(src => src.ObservationPoint));

            //CreateMap<EnergyObservationModel, IEnergyObservation>().ForMember(dest => dest.ObservationPoint, member => member.MapFrom(src => src.ObservationPoint));


            CreateMap<CoordinatesModel, IEnergyObservation>(); //xчто-то странный мапинг потребовал автомапер

            CreateMap<EnergyObservationModel, IEnergyObservation>().IncludeMembers(s => s.ObservationPoint);


            // CreateMap<IEnergyObservation, EnergyObservationModel>();
        }
    }

    //https://stackoverflow.com/questions/40275195/how-to-set-up-automapper-in-asp-net-core
    //https://lostechies.com/jimmybogard/2009/04/15/automapper-feature-interfaces-and-dynamic-mapping/
    //http://docs.automapper.org/en/stable/index.html
}
