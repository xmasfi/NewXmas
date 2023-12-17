using NewShore.Application.flights.Queries.Getflights;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NewShore.Application.Interfaces
{
    public interface INewShoreAIRService
    {
        Task<IList<FlightModel>> GetFlights(CancellationToken cancellationToken);
    }
}
