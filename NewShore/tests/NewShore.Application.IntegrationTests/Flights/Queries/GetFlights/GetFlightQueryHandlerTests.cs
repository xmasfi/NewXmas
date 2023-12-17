using Moq;
using NewShore.Application.flights.Queries.Getflights;
using NewShore.Application.Interfaces;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Asg.Services.ApplicationFramework.Application.Exceptions;
using NewShore.Persistence;
using NewShore.Application.IntegrationTests.Infrastructure;
using MediatR;
using NewShore.Application.Events;
using NewShore.Domain.Entities;
using System.Linq;

namespace NewShore.Application.IntegrationTests.Flights.Queries.GetFlights
{
    public class GetFlightQueryHandlerTests : IntegrationTestBase
    {
        private Mock<INewShoreAIRService> _newShoreAIRServiceMock;
        private GetFlightQueryHandler _getFlightQueryHandler;
        private NewShoreDbContext _dbContext;
        private Mock<IPublisher> _mediatr;


        [SetUp]
        public new void Setup()
        {
            _newShoreAIRServiceMock = new Mock<INewShoreAIRService>(MockBehavior.Strict);
            _dbContext = GetDbContextSqlLite();
            _mediatr = new Mock<IPublisher>();

            _mediatr.Setup(x => x.Publish(It.IsAny<UpdateDB>(), CancellationToken.None));

            _getFlightQueryHandler =
                new GetFlightQueryHandler(_dbContext,
                    _newShoreAIRServiceMock.Object, Mapper, _mediatr.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task ShouldReturnEmptyJourneyWhenNoFlightsReturn()
        {
            // Arrange
            var getFlightQuery = new GetFlightQuery
            {
                Destination = "MAD",
                Origin = "BCN",
                MaxScales = 4                
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlights(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FlightModel>() {  });


            // Act
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _getFlightQueryHandler.Handle(getFlightQuery, CancellationToken.None));

            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlights(It.IsAny<CancellationToken>()), Times.Once);
            _mediatr.Verify(
                x => x.Publish(It.IsAny<UpdateDB>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(ex?.Code, Is.EqualTo("Flights_NotFound"));
            Assert.That(ex?.Message, Is.EqualTo("Entity \"Flights\" () was not found."));
        }


        [Test]
        public async Task ShouldReturnEmptyJourneyWhenNoFlightsWithSameOrigin()
        {
            // Arrange
            var getFlightQuery = new GetFlightQuery
            {
                Destination = "MAD",
                Origin = "BCN",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlights(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FlightModel>() { 
                    new FlightModel()
                    {
                        Origin = "ZZZ",
                        Destination = "MAD",
                        Price = 1.1,
                        Transport = new Domain.ValueObjects.Transport("AAA", "ZZZZ")
                    }
                });


            // Act
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _getFlightQueryHandler.Handle(getFlightQuery, CancellationToken.None));


            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlights(It.IsAny<CancellationToken>()), Times.Once);
            _mediatr.Verify(
                x => x.Publish(It.IsAny<UpdateDB>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(ex?.Code, Is.EqualTo("Flights_NotFound"));
            Assert.That(ex?.Message, Is.EqualTo("Entity \"Flights\" () was not found."));
        }

        [Test]
        public async Task ShouldReturnOneJourneyOneScalesWhenOneFlightsIsCorrect()
        {
            // Arrange
            var getFlightQuery = new GetFlightQuery
            {
                Destination = "MAD",
                Origin = "BCN",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlights(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FlightModel>() {
                    new FlightModel()
                    {
                        Origin = "BCN",
                        Destination = "MAD",
                        Price = 1.1,
                        Transport = new Domain.ValueObjects.Transport("AAA", "ZZZZ")
                    }
                });


            // Act
            var response = await _getFlightQueryHandler.Handle(getFlightQuery, CancellationToken.None);

            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlights(It.IsAny<CancellationToken>()), Times.Once);
            _mediatr.Verify(
                x => x.Publish(It.IsAny<UpdateDB>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(response.Count, Is.EqualTo(1));
            Assert.That(response[0].Flights.Count, Is.EqualTo(1));
            Assert.That(response[0].Origin, Is.EqualTo("BCN"));
            Assert.That(response[0].Destination, Is.EqualTo("MAD"));
            Assert.That(response[0].Price, Is.EqualTo(1.1));
        }

        [Test]
        public async Task ShouldReturnOneJourneyTwoScalesWhenOneFlightsWithTwoScalesIsCorrect()
        {
            // Arrange
            var getFlightQuery = new GetFlightQuery
            {
                Destination = "MAD",
                Origin = "BCN",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlights(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FlightModel>() {
                    new FlightModel()
                    {
                        Origin = "BCN",
                        Destination = "GIRONA",
                        Price = 1.0,
                        Transport = new Domain.ValueObjects.Transport("AAA1", "ZZZZ1")
                    },
                    new FlightModel()
                    {
                        Origin = "GIRONA",
                        Destination = "MAD",
                        Price = 2.0,
                        Transport = new Domain.ValueObjects.Transport("AAA2", "ZZZZ2")
                    }

                });


            // Act
            var response = await _getFlightQueryHandler.Handle(getFlightQuery, CancellationToken.None);

            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlights(It.IsAny<CancellationToken>()), Times.Once);
            _mediatr.Verify(
                x => x.Publish(It.IsAny<UpdateDB>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(response.Count, Is.EqualTo(1));
            Assert.That(response[0].Flights.Count, Is.EqualTo(2));
            Assert.That(response[0].Origin, Is.EqualTo("BCN"));
            Assert.That(response[0].Destination, Is.EqualTo("MAD"));
            Assert.That(response[0].Price, Is.EqualTo(3.0));
        }


        [Test]
        public async Task ShouldReturnTwoJourneyScalesWhenOneFlightsWithTwoScalesIsCorrect()
        {
            // Arrange
            var getFlightQuery = new GetFlightQuery
            {
                Destination = "MAD",
                Origin = "BCN",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlights(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FlightModel>() {
                    new FlightModel()
                    {
                        Origin = "BCN",
                        Destination = "BCN1",
                        Price = 1.0,
                        Transport = new Domain.ValueObjects.Transport("AAA1", "ZZZZ1")
                    },
                    new FlightModel()
                    {
                        Origin = "BCN1",
                        Destination = "MAD",
                        Price = 2.0,
                        Transport = new Domain.ValueObjects.Transport("AAA2", "ZZZZ2")
                    },
                    new FlightModel()
                    {
                        Origin = "BCN",
                        Destination = "BCN2",
                        Price = 3.0,
                        Transport = new Domain.ValueObjects.Transport("AAA1", "ZZZZ1")
                    },
                    new FlightModel()
                    {
                        Origin = "BCN2",
                        Destination = "BCN3",
                        Price = 10.0,
                        Transport = new Domain.ValueObjects.Transport("AAA2", "ZZZZ2")
                    },
                    new FlightModel()
                    {
                        Origin = "BCN3",
                        Destination = "MAD",
                        Price = 100.0,
                        Transport = new Domain.ValueObjects.Transport("AAA2", "ZZZZ2")
                    },
                    new FlightModel()
                    {
                        Origin = "BCN",
                        Destination = "NO_LLEGO1",
                        Price = 1.0,
                        Transport = new Domain.ValueObjects.Transport("AAA1", "ZZZZ1")
                    },
                    new FlightModel()
                    {
                        Origin = "NO_LLEGO1",
                        Destination = "NO_LLEGO2",
                        Price = 2.0,
                        Transport = new Domain.ValueObjects.Transport("AAA2", "ZZZZ2")
                    },
                });


            // Act
            var response = await _getFlightQueryHandler.Handle(getFlightQuery, CancellationToken.None);

            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlights(It.IsAny<CancellationToken>()), Times.Once);
            _mediatr.Verify(
                x => x.Publish(It.IsAny<UpdateDB>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(response.Count, Is.EqualTo(2));

            Assert.That(response[0].Flights.Count, Is.EqualTo(2));
            Assert.That(response[0].Origin, Is.EqualTo("BCN"));
            Assert.That(response[0].Destination, Is.EqualTo("MAD"));
            Assert.That(response[0].Price, Is.EqualTo(3.0));

            Assert.That(response[1].Flights.Count, Is.EqualTo(3));
            Assert.That(response[1].Origin, Is.EqualTo("BCN"));
            Assert.That(response[1].Destination, Is.EqualTo("MAD"));
            Assert.That(response[1].Price, Is.EqualTo(113.0));
        }

        [Test]
        public async Task ShouldReturnValueFromCache()
        {
            // Arrange
            var getFlightQuery = new GetFlightQuery
            {
                Destination = "cacheDest",
                Origin = "cacheOrig",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlights(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FlightModel>() { });

            var journey1 = new Journey()
            {
                Destination = "cacheDest",
                Origin = "cacheOrig",
                Price = 10.2,
                Id = new System.Guid(),
                Flights = new List<Flight>()
                {
                    new Flight()
                    {
                        Destination = "cacheDest",
                        Origin = "cacheOrig",
                        Price = 10.2,
                        Transport = new Domain.ValueObjects.Transport("aaa", "bbb"),
                        Id = new System.Guid()
                    }
                }
            };

            var journey2 = new Journey()
            {
                Destination = "cacheDest",
                Origin = "cacheOrig",
                Price = 40.4,
                Id = new System.Guid(),
                Flights = new List<Flight>()
                {
                    new Flight()
                    {
                        Destination = "cacheDest",
                        Origin = "cacheOrig",
                        Price = 20.2,
                        Transport = new Domain.ValueObjects.Transport("aaa", "bbb"),
                        Id = new System.Guid()
                    },
                    new Flight()
                    {
                        Destination = "cacheDest2",
                        Origin = "cacheOrig2",
                        Price = 20.2,
                        Transport = new Domain.ValueObjects.Transport("aaa", "bbb"),
                        Id = new System.Guid()
                    }
                }
            };

            await _dbContext.journeys.AddAsync(journey1);
            await _dbContext.journeys.AddAsync(journey2);
            await _dbContext.SaveChangesAsync();

            // Act
            var response = await _getFlightQueryHandler.Handle(getFlightQuery, CancellationToken.None);

            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlights(It.IsAny<CancellationToken>()), Times.Never);
            _mediatr.Verify(
                x => x.Publish(It.IsAny<UpdateDB>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.That(response.Count, Is.EqualTo(2));
            Assert.That(response[0].Flights.Count, Is.EqualTo(1));
            Assert.That(response[0].Origin, Is.EqualTo("cacheOrig"));
            Assert.That(response[0].Destination, Is.EqualTo("cacheDest"));
            Assert.That(response[0].Price, Is.EqualTo(10.2));

            Assert.That(response[1].Flights.Count, Is.EqualTo(2));
            Assert.That(response[1].Origin, Is.EqualTo("cacheOrig"));
            Assert.That(response[1].Destination, Is.EqualTo("cacheDest"));
            Assert.That(response[1].Price, Is.EqualTo(40.4));
        }

    }
}
