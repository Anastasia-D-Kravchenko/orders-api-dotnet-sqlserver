using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersApp.Entities;

namespace OrdersApp.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("ID");
        builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Price).IsRequired().HasColumnType("numeric(10,2)");

        builder.HasData(
            new Product { Id = 1, Name = "Bananas", Price = 5.55m },
            new Product { Id = 2, Name = "Orange",  Price = 12.37m }
        );
    }
}
