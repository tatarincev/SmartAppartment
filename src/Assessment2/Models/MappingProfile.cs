using Assessment2.Models.Schema.Index;
using Assessment2.Models.Schema.Json;
using AutoMapper;
using Microsoft.Spatial;

namespace Assessment2.Models
{
    /// <summary>
    /// represents automapper mapping profile 
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RealProperty, ApartmentDataIndexDocument>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PropertyID))
                    .ForMember(dest => dest.Location, opt => opt.MapFrom(src => GeographyPoint.Create((double)src.Lat, (double)src.Lng)));
            CreateMap<MgmntCompany, ApartmentDataIndexDocument>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MgmtID));
            CreateMap<ApartmentDataIndexDocument, ApartmentDataSearchEntry>();
        }
    }
}
