using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersApp.Entities;

namespace OrdersApp.Configurations;

public class ProductOrderConfiguration : IEntityTypeConfiguration<ProductOrder>
{
    public void Configure(EntityTypeBuilder<ProductOrder> builder)
    {
        builder.ToTable("Product_Order");

        // Composite PK
        builder.HasKey(po => new { po.ProductId, po.OrderId });

        builder.Property(po => po.ProductId).HasColumnName("ProductID");
        builder.Property(po => po.OrderId).HasColumnName("OrderID");
        builder.Property(po => po.Amount).IsRequired();

        builder.HasOne(po => po.Product)
               .WithMany(p => p.ProductOrders)
               .HasForeignKey(po => po.ProductId);

        builder.HasOne(po => po.Order)
               .WithMany(o => o.ProductOrders)
               .HasForeignKey(po => po.OrderId);

        // Parents (Client, Status, Product, Order) seeded above
        builder.HasData(
            new ProductOrder { ProductId = 1, OrderId = 2, Amount = 2 },
            new ProductOrder { ProductId = 2, OrderId = 2, Amount = 1 }
        );
    }
}
