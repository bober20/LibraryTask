using Library.Domain.Models;
using Library.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure;

public class LibraryDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}