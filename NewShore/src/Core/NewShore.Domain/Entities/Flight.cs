using Asg.Services.ApplicationFramework.Domain;
using NewShore.Domain.ValueObjects;
using System;

namespace NewShore.Domain.Entities
{
    public class Flight : Entity
    {
        public Transport Transport { get;  set; }
        public string Origin { get;  set; }
        public string Destination { get;  set; }
        public double Price { get;  set; }

        public Flight() { }

        public Flight(Transport transport
            , string origin, string destination, double price) {

            if (string.IsNullOrWhiteSpace(origin))
                throw new ArgumentNullException(nameof(origin));

            if (string.IsNullOrWhiteSpace(destination))
                throw new ArgumentNullException(nameof(destination));

            Transport = transport;
            Origin = origin;
            Destination = destination;
            Price = price;
        }
    }
}