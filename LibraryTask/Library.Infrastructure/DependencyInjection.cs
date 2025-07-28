using Library.Domain.Interfaces;
using Library.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        Action<DbContextOptionsBuilder> options)
    {
        services.AddDbContext<LibraryDbContext>(options);
        
        services.AddTransient<IBookRepository, BookRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        
        return services;
    }
}