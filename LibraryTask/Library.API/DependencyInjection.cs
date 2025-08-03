using Library.API.Interfaces;
using Library.API.Services;
using Library.API.MapperConfigurations;

namespace Library.API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        services.AddMapper();
        
        services.AddTransient<IBookService, BookService>();
        services.AddTransient<IOrderService, OrderService>();
        
        return services;
    }
}