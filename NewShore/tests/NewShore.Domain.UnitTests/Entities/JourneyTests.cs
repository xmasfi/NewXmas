using NewShore.Domain.Entities;
using NewShore.Domain.ValueObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NewShore.DomainInfrastructure.UnitTests.Entities
{
    [TestFixture]
    public class JourneyTests
    {
        [Test]
        public void ShouldBeValidUpdatingTrialWithExpirationTime()
        {
            // Arrange
            var origin = "BCN";
            var destination = "MAD";
            var price = 10.0;
            var flights = new List<Flight>()
            {
                new Flight()
                {
                    Destination = destination,
                    Origin = origin,
                    Price = price,
                    Transport = new Transport("aaa", "bbb")
                }
            };


            // Act
            var journey = new Journey(flights, origin, destination, price);

            // Assert
            Assert.That(journey.Flights.Count, Is.EqualTo(1));
            Assert.That(journey.Origin, Is.EqualTo(origin));
            Assert.That(journey.Destination, Is.EqualTo(destination));
            Assert.That(journey.Price, Is.EqualTo(price));
        }

        [Test]
        public void ShouldThrowArgumentExceptionCreatingTrialWithIdEmpty()
        {
            // Arrange

            // Act & Assert
            Assert.That(
                () => new Journey(null, string.Empty, string.Empty, -1),
                Throws.ArgumentNullException);
        }
    }
}
