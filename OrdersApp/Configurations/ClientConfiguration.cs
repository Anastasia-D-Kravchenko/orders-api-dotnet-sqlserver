using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersApp.Entities;

namespace OrdersApp.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Client");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("ID");
        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(100);

        builder.HasData(
            new Client { Id = 1, FirstName = "John", LastName = "Doe" },
            new Client { Id = 2, FirstName = "Jane", LastName = "Smith" }
        );
    }
}
