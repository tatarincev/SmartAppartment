using AutoMapper;

namespace Assessment1.Models
{
    /// <summary>
    /// represents automapper mapping profile 
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Assessment1.Nswag.Pet, Pet>();          
        }
    }
}
