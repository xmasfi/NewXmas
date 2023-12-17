using NewShore.Domain.ValueObjects;
using System;

namespace NewShore.Application.flights.Queries.Getflights
{
    public class FlightModel
    {
        public Guid Id { get; set; }

        public Transport Transport { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }

    }
}