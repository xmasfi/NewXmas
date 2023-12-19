using Asg.Services.ApplicationFramework.Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewShore.Application.Events;
using NewShore.Application.flights.Queries.Getflights;
using NewShore.Application.Interfaces;
using NewShore.Common.Constants;
using NewShore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewShore.Application.Flights.Queries.GetFlightsWithReturn
{
    public  class GetFlightReturnQueryHandler : IRequestHandler<GetFlightReturnQuery, IList<Journey>>
    {
        private readonly INewShoreDbContext _dbContext;
        private readonly INewShoreAIRService _newShoreAIRService;
        private readonly IMapper _mapper;
        private readonly IPublisher _mediator;


        public GetFlightReturnQueryHandler(INewShoreDbContext dbContext, INewShoreAIRService newShoreAIRService, IMapper mapper, IPublisher mediator)
        {
            _dbContext = dbContext;
            _newShoreAIRService = newShoreAIRService;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IList<Journey>> Handle(GetFlightReturnQuery request, CancellationToken cancellationToken)
        {
            var cache = await _dbContext.journeys.Where(x => x.Origin.Equals(request.Origin) && x.Destination.Equals(request.Destination)).ToListAsync();
            if (cache != null && cache.Any())
            {
                var returnFlights = await _dbContext.journeys.Where(x => x.Origin.Equals(request.Destination) && x.Destination.Equals(request.Origin)).ToListAsync();
                cache.AddRange(returnFlights);
                return cache;
            }

            var flights = await _newShoreAIRService.GetFlightsReturn(cancellationToken);


            var finalFlights = computeJourney(flights, new List<FlightModel>(), request.Origin, request.Destination, request.MaxScales);

            var journeys = new List<Journey>();

            foreach (Journey journey in finalFlights)
            {
                var partialJourney = new Journey
                {
                    Origin = request.Origin,
                    Destination = request.Destination,
                    Flights = journey.Flights,
                    Price = journey.Flights.Sum(item => item.Price)
                };

                journeys.Add(partialJourney);
            }

            // Si hay vuelos de ida, mirar si hay vuelos de vuelta
            if (finalFlights.Any())
            {
                var finalReturnFlights = computeJourney(flights, new List<FlightModel>(), request.Destination, request.Origin, request.MaxScales);

                foreach (Journey journey in finalReturnFlights)
                {
                    var partialJourney = new Journey
                    {
                        Origin = request.Destination,
                        Destination = request.Origin,
                        Flights = journey.Flights,
                        Price = journey.Flights.Sum(item => item.Price)
                    };

                    journeys.Add(partialJourney);
                }
            }


            // Evento de sistema para guardar a BD la consulta realizada y que tiene info correcta
            var updateDB = new UpdateDB(journeys);
            await _mediator.Publish(updateDB);


            if (journeys.Any() != true)
            {
                // Send exception not found
                throw new NotFoundException("Flights", "", $"Flights_{ErrorCodes.NotFound}");
            }

            return journeys;
        }


        // allFlightsPending  : Contiene la lista de escalas a comprobar si nos llevaran a nuestro destino
        // allScalesUsed      : Contine la lista de escalas que necesitamos parcialmente para legar a nuestro destino

        // origin             : Origin donde estamos (puede ser el inicial o donde nos ha llevado una escala de avion)
        // destination        : Destino final (inmutable) 
        private List<Journey> computeJourney(IList<FlightModel> allFlightsPending, IList<FlightModel> allScalesUsed, string origin, string destination, int maxScales)
        {
            var airplaneScales = allFlightsPending.Where(x => x.Origin.Equals(origin));

            var journeys = new List<Journey>();

            if (airplaneScales.Any())
            {
                foreach (var scale in airplaneScales)
                {
                    var scalesUsed = new List<FlightModel>();
                    scalesUsed.AddRange(allScalesUsed);
                    scalesUsed.Add(scale);

                    if (scale.Destination.Equals(destination))
                    {
                        // hemos encontrado el destino, paramos de buscar
                        var partialJourney = new Journey
                        {
                            Origin = origin,
                            Destination = destination,
                            Flights = _mapper.Map<List<Flight>>(scalesUsed),
                            Price = scale.Price
                        };

                        journeys.Add(partialJourney);
                    }
                    else
                    {
                        // seguimos buscando el destino en el resto de escalas de la lista
                        // Borrar la escala, pues no nos lleva donde queremos
                        if (maxScales > 0)
                        {
                            journeys.AddRange(computeJourney(allFlightsPending, scalesUsed, scale.Destination, destination, maxScales - 1));
                        }
                    }
                }
            }

            return journeys;
        }
    }
}
