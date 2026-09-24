using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersApp.Entities;

namespace OrdersApp.Configurations;

public class StatusConfiguration : IEntityTypeConfiguration<Status>
{
    public void Configure(EntityTypeBuilder<Status> builder)
    {
        builder.ToTable("Status");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("ID");
        builder.Property(s => s.Name).IsRequired().HasMaxLength(50);

        builder.HasData(
            new Status { Id = 1, Name = "Ongoing" },
            new Status { Id = 2, Name = "Completed" },
            new Status { Id = 3, Name = "Cancelled" }
        );
    }
}
