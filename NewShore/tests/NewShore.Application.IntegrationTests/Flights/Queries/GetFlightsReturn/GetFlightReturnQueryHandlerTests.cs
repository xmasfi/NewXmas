using Asg.Services.ApplicationFramework.Application.Exceptions;
using MediatR;
using Moq;
using NewShore.Application.Events;
using NewShore.Application.flights.Queries.Getflights;
using NewShore.Application.Flights.Queries.GetFlightsWithReturn;
using NewShore.Application.IntegrationTests.Infrastructure;
using NewShore.Application.Interfaces;
using NewShore.Domain.Entities;
using NewShore.Persistence;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NewShore.Application.IntegrationTests.Flights.Queries.GetFlightsWithReturn
{
    public class GetFlightReturnQueryHandlerTests : IntegrationTestBase
    {
        private Mock<INewShoreAIRService> _newShoreAIRServiceMock;
        private GetFlightReturnQueryHandler _getFlightReturnQueryHandler;
        private NewShoreDbContext _dbContext;
        private Mock<IPublisher> _mediatr;


        [SetUp]
        public new void Setup()
        {
            _newShoreAIRServiceMock = new Mock<INewShoreAIRService>(MockBehavior.Strict);
            _dbContext = GetDbContextSqlLite();
            _mediatr = new Mock<IPublisher>();

            _mediatr.Setup(x => x.Publish(It.IsAny<UpdateDB>(), CancellationToken.None));

            _getFlightReturnQueryHandler =
                new GetFlightReturnQueryHandler(_dbContext,
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
            var getFlightReturnQuery = new GetFlightReturnQuery
            {
                Destination = "MAD",
                Origin = "BCN",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlightsReturn(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FlightModel>() { });


            // Act
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _getFlightReturnQueryHandler.Handle(getFlightReturnQuery, CancellationToken.None));

            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlightsReturn(It.IsAny<CancellationToken>()), Times.Once);
            _mediatr.Verify(
                x => x.Publish(It.IsAny<UpdateDB>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(ex?.Code, Is.EqualTo("Flights_NotFound"));
            Assert.That(ex?.Message, Is.EqualTo("Entity \"Flights\" () was not found."));
        }


        [Test]
        public async Task ShouldReturnEmptyJourneyWhenNoFlightsWithSameOrigin()
        {
            // Arrange
            var getFlightReturnQuery = new GetFlightReturnQuery
            {
                Destination = "MAD",
                Origin = "BCN",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlightsReturn(It.IsAny<CancellationToken>()))
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
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _getFlightReturnQueryHandler.Handle(getFlightReturnQuery, CancellationToken.None));


            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlightsReturn(It.IsAny<CancellationToken>()), Times.Once);
            _mediatr.Verify(
                x => x.Publish(It.IsAny<UpdateDB>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(ex?.Code, Is.EqualTo("Flights_NotFound"));
            Assert.That(ex?.Message, Is.EqualTo("Entity \"Flights\" () was not found."));
        }

        [Test]
        public async Task ShouldReturnOneJourneyOneScalesWhenOneFlightsIsCorrect()
        {
            // Arrange
            var getFlightReturnQuery = new GetFlightReturnQuery
            {
                Destination = "MAD",
                Origin = "BCN",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlightsReturn(It.IsAny<CancellationToken>()))
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
            var response = await _getFlightReturnQueryHandler.Handle(getFlightReturnQuery, CancellationToken.None);

            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlightsReturn(It.IsAny<CancellationToken>()), Times.Once);
            _mediatr.Verify(
                x => x.Publish(It.IsAny<UpdateDB>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(response.Count, Is.EqualTo(1));
            Assert.That(response[0].Flights.Count, Is.EqualTo(1));
            Assert.That(response[0].Origin, Is.EqualTo("BCN"));
            Assert.That(response[0].Destination, Is.EqualTo("MAD"));
            Assert.That(response[0].Price, Is.EqualTo(1.1));
        }

        [Test]
        public async Task ShouldReturnOneJourneyOneReturnOneScalesWhenOneFlightsIsCorrect()
        {
            // Arrange
            var getFlightReturnQuery = new GetFlightReturnQuery
            {
                Destination = "MAD",
                Origin = "BCN",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlightsReturn(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FlightModel>() {
                    new FlightModel()
                    {
                        Origin = "BCN",
                        Destination = "MAD",
                        Price = 1.1,
                        Transport = new Domain.ValueObjects.Transport("AAA", "ZZZZ")
                    },
                    new FlightModel()
                    {
                        Origin = "MAD",
                        Destination = "BCN",
                        Price = 2.2,
                        Transport = new Domain.ValueObjects.Transport("AAA1", "ZZZZ2")
                    }
                });


            // Act
            var response = await _getFlightReturnQueryHandler.Handle(getFlightReturnQuery, CancellationToken.None);

            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlightsReturn(It.IsAny<CancellationToken>()), Times.Once);
            _mediatr.Verify(
                x => x.Publish(It.IsAny<UpdateDB>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(response.Count, Is.EqualTo(2));
            Assert.That(response[0].Flights.Count, Is.EqualTo(1));
            Assert.That(response[0].Origin, Is.EqualTo("BCN"));
            Assert.That(response[0].Destination, Is.EqualTo("MAD"));
            Assert.That(response[0].Price, Is.EqualTo(1.1));

            Assert.That(response[0].Flights[0].Origin, Is.EqualTo("BCN"));
            Assert.That(response[0].Flights[0].Destination, Is.EqualTo("MAD"));
            Assert.That(response[0].Flights[0].Price, Is.EqualTo(1.1));

            Assert.That(response[1].Flights.Count, Is.EqualTo(1));
            Assert.That(response[1].Origin, Is.EqualTo("MAD"));
            Assert.That(response[1].Destination, Is.EqualTo("BCN"));
            Assert.That(response[1].Price, Is.EqualTo(2.2));

            Assert.That(response[1].Flights[0].Origin, Is.EqualTo("MAD"));
            Assert.That(response[1].Flights[0].Destination, Is.EqualTo("BCN"));
            Assert.That(response[1].Flights[0].Price, Is.EqualTo(2.2));
        }

        [Test]
        public async Task ShouldReturnOneJourneyTwoScalesWhenOneFlightsWithTwoScalesIsCorrect()
        {
            // Arrange
            var getFlightReturnQuery = new GetFlightReturnQuery
            {
                Destination = "MAD",
                Origin = "BCN",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlightsReturn(It.IsAny<CancellationToken>()))
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
            var response = await _getFlightReturnQueryHandler.Handle(getFlightReturnQuery, CancellationToken.None);

            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlightsReturn(It.IsAny<CancellationToken>()), Times.Once);
            _mediatr.Verify(
                x => x.Publish(It.IsAny<UpdateDB>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(response.Count, Is.EqualTo(1));
            Assert.That(response[0].Flights.Count, Is.EqualTo(2));
            Assert.That(response[0].Origin, Is.EqualTo("BCN"));
            Assert.That(response[0].Destination, Is.EqualTo("MAD"));
            Assert.That(response[0].Price, Is.EqualTo(3.0));

            Assert.That(response[0].Flights[0].Origin, Is.EqualTo("BCN"));
            Assert.That(response[0].Flights[0].Destination, Is.EqualTo("GIRONA"));
            Assert.That(response[0].Flights[1].Origin, Is.EqualTo("GIRONA"));
            Assert.That(response[0].Flights[1].Destination, Is.EqualTo("MAD"));
        }

        [Test]
        public async Task ShouldReturnOneJourneyTwoScalesWithReturnWhenOneFlightsWithTwoScalesIsCorrect()
        {
            // Arrange
            var getFlightReturnQuery = new GetFlightReturnQuery
            {
                Destination = "MAD",
                Origin = "BCN",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlightsReturn(It.IsAny<CancellationToken>()))
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
                    },
                    new FlightModel()
                    {
                        Origin = "MAD",
                        Destination = "TARRACO",
                        Price = 1.0,
                        Transport = new Domain.ValueObjects.Transport("AAA1", "ZZZZ1")
                    },
                    new FlightModel()
                    {
                        Origin = "TARRACO",
                        Destination = "BCN",
                        Price = 2.0,
                        Transport = new Domain.ValueObjects.Transport("AAA2", "ZZZZ2")
                    }
                });


            // Act
            var response = await _getFlightReturnQueryHandler.Handle(getFlightReturnQuery, CancellationToken.None);

            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlightsReturn(It.IsAny<CancellationToken>()), Times.Once);
            _mediatr.Verify(
                x => x.Publish(It.IsAny<UpdateDB>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.That(response.Count, Is.EqualTo(2));
            Assert.That(response[0].Flights.Count, Is.EqualTo(2));
            Assert.That(response[0].Origin, Is.EqualTo("BCN"));
            Assert.That(response[0].Destination, Is.EqualTo("MAD"));
            Assert.That(response[0].Price, Is.EqualTo(3.0));

            Assert.That(response[0].Flights[0].Origin, Is.EqualTo("BCN"));
            Assert.That(response[0].Flights[0].Destination, Is.EqualTo("GIRONA"));
            Assert.That(response[0].Flights[1].Origin, Is.EqualTo("GIRONA"));
            Assert.That(response[0].Flights[1].Destination, Is.EqualTo("MAD"));

            Assert.That(response[1].Flights.Count, Is.EqualTo(2));
            Assert.That(response[1].Origin, Is.EqualTo("MAD"));
            Assert.That(response[1].Destination, Is.EqualTo("BCN"));
            Assert.That(response[1].Price, Is.EqualTo(3.0));

            Assert.That(response[1].Flights[0].Origin, Is.EqualTo("MAD"));
            Assert.That(response[1].Flights[0].Destination, Is.EqualTo("TARRACO"));
            Assert.That(response[1].Flights[1].Origin, Is.EqualTo("TARRACO"));
            Assert.That(response[1].Flights[1].Destination, Is.EqualTo("BCN"));
        }

        [Test]
        public async Task ShouldReturnTwoJourneyScalesWhenOneFlightsWithTwoScalesIsCorrect()
        {
            // Arrange
            var getFlightReturnQuery = new GetFlightReturnQuery
            {
                Destination = "MAD",
                Origin = "BCN",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlightsReturn(It.IsAny<CancellationToken>()))
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
            var response = await _getFlightReturnQueryHandler.Handle(getFlightReturnQuery, CancellationToken.None);

            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlightsReturn(It.IsAny<CancellationToken>()), Times.Once);
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
            var getFlightReturnQuery = new GetFlightReturnQuery
            {
                Destination = "cacheDest",
                Origin = "cacheOrig",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlightsReturn(It.IsAny<CancellationToken>()))
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

            var journey3 = new Journey()
            {
                Destination = "pipo",
                Origin = "pupa",
                Price = 40.4,
                Id = new System.Guid(),
                Flights = new List<Flight>()
            };

            await _dbContext.journeys.AddAsync(journey1);
            await _dbContext.journeys.AddAsync(journey2);
            await _dbContext.journeys.AddAsync(journey3);
            await _dbContext.SaveChangesAsync();

            // Act
            var response = await _getFlightReturnQueryHandler.Handle(getFlightReturnQuery, CancellationToken.None);

            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlightsReturn(It.IsAny<CancellationToken>()), Times.Never);
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

        [Test]
        public async Task ShouldReturnValueWithReturnFromCache()
        {
            // Arrange
            var getFlightReturnQuery = new GetFlightReturnQuery
            {
                Destination = "cacheDest",
                Origin = "cacheOrig",
                MaxScales = 4
            };

            _newShoreAIRServiceMock.Setup(x =>
                x.GetFlightsReturn(It.IsAny<CancellationToken>()))
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

            var journey3 = new Journey()
            {
                Destination = "cacheOrig",
                Origin = "cacheDest",
                Price = 40.4,
                Id = new System.Guid(),
                Flights = new List<Flight>()
                {
                    new Flight()
                    {
                        Destination = "cacheOrig",
                        Origin = "cacheDest",
                        Price = 20.2,
                        Transport = new Domain.ValueObjects.Transport("aaa", "bbb"),
                        Id = new System.Guid()
                    },
                    new Flight()
                    {
                        Destination = "cacheOrig2",
                        Origin = "cacheDest2",
                        Price = 20.2,
                        Transport = new Domain.ValueObjects.Transport("aaa", "bbb"),
                        Id = new System.Guid()
                    }
                }
            };

            var journey4 = new Journey()
            {
                Destination = "pipo",
                Origin = "pupa",
                Price = 40.4,
                Id = new System.Guid(),
                Flights = new List<Flight>()
            };

            await _dbContext.journeys.AddAsync(journey1);
            await _dbContext.journeys.AddAsync(journey2);
            await _dbContext.journeys.AddAsync(journey3);
            await _dbContext.journeys.AddAsync(journey4);
            await _dbContext.SaveChangesAsync();

            // Act
            var response = await _getFlightReturnQueryHandler.Handle(getFlightReturnQuery, CancellationToken.None);

            // Assert
            _newShoreAIRServiceMock.Verify(
                x => x.GetFlightsReturn(It.IsAny<CancellationToken>()), Times.Never);
            _mediatr.Verify(
                x => x.Publish(It.IsAny<UpdateDB>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.That(response.Count, Is.EqualTo(3));
            Assert.That(response[0].Flights.Count, Is.EqualTo(1));
            Assert.That(response[0].Origin, Is.EqualTo("cacheOrig"));
            Assert.That(response[0].Destination, Is.EqualTo("cacheDest"));
            Assert.That(response[0].Price, Is.EqualTo(10.2));

            Assert.That(response[1].Flights.Count, Is.EqualTo(2));
            Assert.That(response[1].Origin, Is.EqualTo("cacheOrig"));
            Assert.That(response[1].Destination, Is.EqualTo("cacheDest"));
            Assert.That(response[1].Price, Is.EqualTo(40.4));

            Assert.That(response[2].Flights.Count, Is.EqualTo(2));
            Assert.That(response[2].Origin, Is.EqualTo("cacheDest"));
            Assert.That(response[2].Destination, Is.EqualTo("cacheOrig"));
            Assert.That(response[2].Price, Is.EqualTo(40.4));
        }
    }
}
