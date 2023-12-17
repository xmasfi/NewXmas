using NUnit.Framework;
using NewShore.Domain.ValueObjects;

namespace NewShore.DomainInfrastructure.UnitTests.ValueObjects
{
    public class TransportTests
    {
        [Test]
        public void CanBeCreatedWhenCorrectValueAndReturnsLowerCase()
        {
            // Arrange 
            var flightCarrier = "aaaa";
            var flightNumber = "bbbb";

            // Act
            var correctTransport = new Transport(flightCarrier, flightNumber);

            //Assert
            Assert.That(correctTransport.FlightNumber, Is.EqualTo("bbbb"));
            Assert.That(correctTransport.FlightCarrier, Is.EqualTo("aaaa"));
        }


        [Test]
        public void ShouldThrowExceptionWhenValueIsNullOrEmpty()
        {
            // Arrange 
            var flightCarrier = "aaaa";
            var flightNumber = "bbbb";

            //Act && Assert
            Assert.That(() => new Transport(flightCarrier, null), Throws.ArgumentNullException);
            Assert.That(() => new Transport(string.Empty, flightNumber), Throws.ArgumentNullException);
        }
    }
}
