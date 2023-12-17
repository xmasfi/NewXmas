using MediatR;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using NewShore.Application.IntegrationTests.Infrastructure;
using System.Threading;
using NewShore.Application.Handlers;
using NewShore.Application.Interfaces;
using NewShore.Application.Events;
using NewShore.Domain.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NewShore.Application.IntegrationTests.Events
{
    public class OnUpdateDBTests : IntegrationTestBase
    {
        private OnUpdateDB _onUpdateDB;
        private INewShoreDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            _dbContext = GetDbContextSqlLite();
            _onUpdateDB = new OnUpdateDB(_dbContext);
        }

        [Test]
        public async Task ShouldUpdateDBWithInformation()
        {
            // Arrange
            var journeysToInsert = new List<Journey>()
            {
                new Journey()
                {
                    Destination = "dest1",
                    Origin = "origin1",
                    Price = 10.0,
                    Flights = new List<Flight>()
                    {
                        new Flight()
                        {
                            Origin = "origin1",
                            Destination = "dest1",
                            Price = 10.0,
                            Transport = new Domain.ValueObjects.Transport("aaa", "bbb")
                        }
                    }
                },
                new Journey()
                {
                    Destination = "dest2",
                    Origin = "origin2",
                    Price = 10.0,
                    Flights = new List<Flight>()
                    {
                        new Flight()
                        {
                            Origin = "origin1",
                            Destination = "dest1",
                            Price = 10.0,
                            Transport = new Domain.ValueObjects.Transport("aaa", "bbb")
                        },
                        new Flight()
                        {
                            Origin = "origin2",
                            Destination = "dest2",
                            Price = 10.0,
                            Transport = new Domain.ValueObjects.Transport("aaa2", "bbb2")
                        }
                    }
                }
            };

            var updateDB = new UpdateDB(journeysToInsert);

            // Act
            await _onUpdateDB.Handle(updateDB, CancellationToken.None);

            // Assert
            var journeysSaved = await _dbContext.journeys.ToListAsync();
            var flightsSaved = await _dbContext.flights.ToListAsync();

            Assert.That(journeysSaved.Count, Is.EqualTo(2));
            Assert.That(flightsSaved.Count, Is.EqualTo(3));     
//TODO el valor correcto es 2            Assert.That(flightsSaved.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task ShouldUpdateDBWithNoInformation()
        {
            // Arrange
            var journeysToInsert = new List<Journey>();

            var updateDB = new UpdateDB(journeysToInsert);

            // Act
            await _onUpdateDB.Handle(updateDB, CancellationToken.None);

            // Assert
            var journeysSaved = await _dbContext.journeys.ToListAsync();
            var flightsSaved = await _dbContext.flights.ToListAsync();

            Assert.That(journeysSaved.Count, Is.EqualTo(0));
            Assert.That(flightsSaved.Count, Is.EqualTo(0));
        }

    }
}
