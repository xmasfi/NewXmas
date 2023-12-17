using NewShore.Domain.UnitTests.Infrastructure;
using NewShore.Infrastructure.AutoMapper;
using NewShore.Infrastructure.Services;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using System.Collections.Generic;
using NewShoreAIR.Api.Client.Contracts;

namespace NewShore.Domain.UnitTests.Services
{
    public  class NewShoreAIRServiceTests : InfrastructureTestsBase<InfrastructureProfile>
    {
        private NewShoreAIRService _newShoreAIRService;
        private Mock<INewShoreAIRClient> _newShoreAIRClientMock;

        [SetUp]
        public new void Setup()
        {
            _newShoreAIRClientMock = new Mock<INewShoreAIRClient>();


            _newShoreAIRClientMock.Setup(x => x.GetFlightsAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                new List<FlightRepresentation>()
                {
                    new FlightRepresentation()
                    {
                        DepartureStation = "BCN",
                        ArrivalStation = "MAD",
                        FlightCarrier = "ZZZ12",
                        FlightNumber = "AA123",
                        Price = 10.0
                    }
                });

            _newShoreAIRService = new NewShoreAIRService(_newShoreAIRClientMock.Object, Mapper);
        }


        [Test]
        public async Task ShouldCreateKeycloakClientProperly()
        {
            // Arrange

            // Act
            var response = await _newShoreAIRService.GetFlights();

            // Assert
            _newShoreAIRClientMock.Verify(x => x.GetFlightsAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Count, Is.EqualTo(1));
            Assert.That(response[0].Destination, Is.EqualTo("MAD"));
            Assert.That(response[0].Origin, Is.EqualTo("BCN"));
            Assert.That(response[0].Transport.FlightNumber, Is.EqualTo("AA123"));
            Assert.That(response[0].Transport.FlightCarrier, Is.EqualTo("ZZZ12"));
            Assert.That(response[0].Price, Is.EqualTo(10.0));

        }

    }
}
