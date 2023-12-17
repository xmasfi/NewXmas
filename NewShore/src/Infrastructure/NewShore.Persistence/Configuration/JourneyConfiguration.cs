using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewShore.Domain.Entities;
using NewShore.Persistence.Infrastructure;

namespace NewShore.Persistence.Configuration
{
    internal class JourneyConfiguration : IEntityTypeConfiguration<Journey>
    {
        public void Configure(EntityTypeBuilder<Journey> builder)
        {
            builder.Property(e => e.Origin)
                .IsRequired()
                .HasMaxLength(Constants.DefaultStringLength);

            builder.Property(e => e.Destination)
                .IsRequired()
                .HasMaxLength(Constants.DefaultStringLength);

            builder.Property(e => e.Price)
                .IsRequired();

            builder.HasMany(e => e.Flights)
                .WithOne()
                .IsRequired();
        }

    }
}
