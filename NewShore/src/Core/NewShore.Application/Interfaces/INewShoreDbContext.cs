using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewShore.Domain.Entities;

namespace NewShore.Application.Interfaces
{
    public interface INewShoreDbContext
    {
        DbSet<Flight> flights { get; set; }
        DbSet<Journey> journeys { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
