using MediatR;
using NewShore.Domain.Entities;
using System.Collections.Generic;


namespace NewShore.Application.Flights.Queries.GetFlightsWithReturn
{
    public class GetFlightReturnQuery : IRequest<IList<Journey>>
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int MaxScales { get; set; }
    }
}
