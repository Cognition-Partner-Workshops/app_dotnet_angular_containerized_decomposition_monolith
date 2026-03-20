using AutoMapper;
using ShopService.API.ViewModels;
using ShopService.Core.Models;

namespace ShopService.API.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerVM>()
                .ReverseMap();

            CreateMap<Product, ProductVM>()
                .ReverseMap();

            CreateMap<Order, OrderVM>()
                .ReverseMap();
        }
    }
}
