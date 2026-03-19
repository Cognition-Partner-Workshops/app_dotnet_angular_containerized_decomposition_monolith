using AutoMapper;
using CustomerMicroservice.Models;
using CustomerMicroservice.ViewModels;

namespace CustomerMicroservice.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerVM>()
                .ReverseMap();
        }
    }
}
