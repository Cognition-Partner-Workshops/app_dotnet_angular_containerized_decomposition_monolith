using AutoMapper;
using OrderService.Models;
using OrderService.ViewModels;

namespace OrderService.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderVM>()
                .ReverseMap();

            CreateMap<OrderDetail, OrderDetailVM>()
                .ReverseMap();
        }
    }
}
