using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DateOnlyConverter = Library.Infrastructure.Converters.DateOnlyConverter;

namespace Library.Infrastructure.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .HasOne(b => b.Order)
            .WithMany(o => o.Books)
            .HasForeignKey(b => b.OrderId);
        
        builder.Property(x => x.PublishDate)
            .HasConversion<DateOnlyConverter>();
    }
}