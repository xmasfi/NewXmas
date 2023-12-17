using MediatR;
using NewShore.Domain.Entities;
using System.Collections.Generic;

namespace NewShore.Application.flights.Queries.Getflights
{
    public class GetFlightQuery : IRequest<IList<Journey>>
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int MaxScales { get; set; }
    }
}