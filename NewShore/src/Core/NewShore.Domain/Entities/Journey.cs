using Asg.Services.ApplicationFramework.Domain;
using System;
using System.Collections.Generic;

namespace NewShore.Domain.Entities
{
    public  class Journey : Entity
    {
        public List <Flight> Flights { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }

        public Journey()
        {
            Price = 0.0;
            Flights = null;
            Origin = "";
            Destination = "";
        }

        public Journey(List<Flight> flights, string origin, string destination, double price)
        {

            if (string.IsNullOrWhiteSpace(origin))
                throw new ArgumentNullException(nameof(origin));

            if (string.IsNullOrWhiteSpace(destination))
                throw new ArgumentNullException(nameof(destination));

            Flights = flights;
            Origin = origin;
            Destination = destination;
            Price = price;
        }

    }
}
