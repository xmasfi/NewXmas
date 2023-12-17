using AutoMapper;
using NewShore.Application.flights.Queries.Getflights;
using NewShore.Application.Interfaces;
using NewShoreAIR.Api.Client.Contracts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace NewShore.Infrastructure.Services
{
    public class NewShoreAIRService : INewShoreAIRService
    {
        private readonly INewShoreAIRClient _newShoreAIRClient;
        private readonly IMapper _mapper;
        private const int difficult = 1;


        public NewShoreAIRService(INewShoreAIRClient newShoreAIRClient, IMapper mapper)
        {
            _newShoreAIRClient = newShoreAIRClient;
            _mapper = mapper;
        }


        public async Task<IList<FlightModel>> GetFlights(CancellationToken cancellationToken = default)
        {
            var flights = await _newShoreAIRClient.GetFlightsAsync(difficult, cancellationToken);

            return _mapper.Map<List<FlightModel>>(flights);
        }
    }
}
