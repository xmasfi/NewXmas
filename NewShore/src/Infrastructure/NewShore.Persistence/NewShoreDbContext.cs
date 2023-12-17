using Microsoft.EntityFrameworkCore;
using NewShore.Application.Interfaces;
using NewShore.Domain.Entities;
using NewShore.Persistence.Infrastructure;
using System.Threading.Tasks;
using System.Threading;

namespace NewShore.Persistence
{
    public class NewShoreDbContext : TransactionDbContext, INewShoreDbContext
    {
        public NewShoreDbContext(DbContextOptions<NewShoreDbContext> options) : base(options)
        {
        }

        public DbSet<Flight> flights { get; set; }
        public DbSet<Journey> journeys { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseIdentityColumns();
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

    }
}