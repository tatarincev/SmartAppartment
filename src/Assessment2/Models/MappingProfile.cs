using Assessment2.Models.Index;
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
            CreateMap<RealProperty, IndexDocument>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PropertyID))
                    .ForMember(dest => dest.Location, opt => opt.MapFrom(src => GeographyPoint.Create((double)src.Lat, (double)src.Lng)))
                    .ForMember(dest => dest.DocType, opt => opt.MapFrom(src=> "property"));
            CreateMap<ManagementCompany, IndexDocument>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MgmtID))
                .ForMember(dest => dest.DocType, opt => opt.MapFrom(src => "company"));
        }
    }
}
