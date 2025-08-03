using AutoMapper;
using Library.API.DTOs;
using Library.Domain.Models;

namespace Library.API.MapperConfigurations;

public static class MapperConfigurations
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        var configurations = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Book, BookDTO>()
                .ReverseMap();
            cfg.CreateMap<BookDTO, Book>()
                .ReverseMap();
            
            cfg.CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Books, 
                    opt => opt.MapFrom(src => src.Books))
                .ReverseMap();
                    
            cfg.CreateMap<OrderDTO, Order>()
                .ForMember(dest => dest.Books, 
                    opt => opt.MapFrom(src => src.Books))
                .ReverseMap();
        }, new LoggerFactory());
        
        configurations.CreateMapper();
        services.AddSingleton(configurations.CreateMapper());
        return services;
    }
}