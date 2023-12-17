using Asg.Services.ApplicationFramework.Domain;
using System;
using System.Collections.Generic;

namespace NewShore.Domain.ValueObjects
{
    public class Transport : ValueObject
    {
        public Transport(string flightCarrier, string flightNumber) {

            if (string.IsNullOrWhiteSpace(flightCarrier))
                throw new ArgumentNullException(nameof(flightCarrier));

            if (string.IsNullOrWhiteSpace(flightNumber))
                throw new ArgumentNullException(nameof(flightNumber));

            FlightCarrier = flightCarrier;
            FlightNumber = flightNumber;
        }

        public string FlightCarrier { get; set; }
        public string FlightNumber { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FlightCarrier;
            yield return FlightNumber;
        }
        public override string ToString()
        {
            return FlightCarrier + "::" + FlightNumber;
        }
    }
}
