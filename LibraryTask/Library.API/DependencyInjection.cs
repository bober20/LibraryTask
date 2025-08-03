using Library.API.Interfaces;
using Library.API.Services;
using Library.API.MapperConfigurations;

namespace Library.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        services.AddMapper();
        
        services.AddSingleton<IBookService, BookService>();
        services.AddSingleton<IOrderService, OrderService>();
        
        return services;
    }
}