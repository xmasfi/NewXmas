using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewShore.Domain.Entities;
using NewShore.Domain.ValueObjects;
using NewShore.Persistence.Infrastructure;


namespace NewShore.Persistence.Configuration
{
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.Property(e => e.Origin)
                .IsRequired()
                .HasMaxLength(Constants.DefaultStringLength);

            builder.Property(e => e.Destination)
                .IsRequired()
                .HasMaxLength(Constants.DefaultStringLength);

            builder.Property(e => e.Price)
                .IsRequired()
                .HasMaxLength(Constants.DefaultStringLength);

            builder.OwnsOne<Transport>("Transport", e =>
            {
                e.WithOwner().HasForeignKey("FlightId");
                e.ToTable("Transport");
                e.HasKey("FlightId");
            });
                
        }

    }
}
