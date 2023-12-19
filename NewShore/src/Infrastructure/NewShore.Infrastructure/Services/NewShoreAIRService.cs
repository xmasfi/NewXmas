using AutoMapper;
using NewShore.Application.flights.Queries.Getflights;
using NewShore.Application.Interfaces;
using NewShoreAIR.Api.Client.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace NewShore.Infrastructure.Services
{
    public class NewShoreAIRService : INewShoreAIRService
    {
        private readonly INewShoreAIRClient _newShoreAIRClient;
        private readonly IMapper _mapper;
        private const int difficult = 1;
        private const int difficultReturn = 2;
        private List<FlightModel> _flights;
        private List<FlightModel> _flightsReturn;


        public NewShoreAIRService(INewShoreAIRClient newShoreAIRClient, IMapper mapper)
        {
            _newShoreAIRClient = newShoreAIRClient;
            _mapper = mapper;

            _flights = new List<FlightModel>();
            _flightsReturn = new List<FlightModel>();
        }


        public async Task<IList<FlightModel>> GetFlights(CancellationToken cancellationToken = default)
        {
            if (_flights.Any())
                return _flights;

            var flights = await _newShoreAIRClient.GetFlightsAsync(difficult, cancellationToken);

            _flights.AddRange(_mapper.Map<List<FlightModel>>(flights));
            return _flights;
        }
        public async Task<IList<FlightModel>> GetFlightsReturn(CancellationToken cancellationToken = default)
        {
            if (_flightsReturn.Any())
                return _flightsReturn;
            
            var flights = await _newShoreAIRClient.GetFlightsAsync(difficultReturn, cancellationToken);

            _flightsReturn.AddRange(_mapper.Map<List<FlightModel>>(flights));
            return _flightsReturn;
        }
    }
}
