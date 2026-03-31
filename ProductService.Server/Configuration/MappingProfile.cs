using AutoMapper;
using ProductService.Core.Models;
using ProductService.Server.ViewModels;

namespace ProductService.Server.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductVM>()
                .ReverseMap();
        }
    }
}
