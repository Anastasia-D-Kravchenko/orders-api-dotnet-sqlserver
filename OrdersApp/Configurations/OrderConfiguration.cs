using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersApp.Entities;

namespace OrdersApp.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasColumnName("ID");
        builder.Property(o => o.CreatedAt).IsRequired();
        // FulfilledAt is nullable → no IsRequired()
        builder.Property(o => o.FulfilledAt);
        builder.Property(o => o.ClientId).HasColumnName("ClientID").IsRequired();
        builder.Property(o => o.StatusId).HasColumnName("StatusID").IsRequired();

        builder.HasOne(o => o.Client)
               .WithMany(c => c.Orders)
               .HasForeignKey(o => o.ClientId);

        builder.HasOne(o => o.Status)
               .WithMany(s => s.Orders)
               .HasForeignKey(o => o.StatusId);

        builder.HasData(
            new Order { Id = 1, CreatedAt = new DateTime(2025, 5, 1), FulfilledAt = null, ClientId = 1, StatusId = 1 },
            new Order { Id = 2, CreatedAt = new DateTime(2025, 5, 2), FulfilledAt = null, ClientId = 1, StatusId = 1 }
        );
    }
}
