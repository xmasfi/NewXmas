using AutoFixture;
using NewShore.Application.flights.Queries.Getflights;
using NewShore.Domain.Entities;
using NewShore.Domain.UnitTests.Infrastructure;
using NewShore.Domain.ValueObjects;
using NewShore.Infrastructure.AutoMapper;
using NewShoreAIR.Api.Client.Contracts;
using NUnit.Framework;
using System;

namespace NewShore.Domain.UnitTests.Mappings
{
    public class NewShoreAIRServiceInfrastructureTests : InfrastructureTestsBase<InfrastructureProfile>
    {
        [Test]
        public void ShouldMapApiClientApplicationRegistration1()
        {
            var entity = Fixture.Build<FlightModel>()
                .With(x => x.Transport, new Transport(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()))
                .With(x => x.Origin, Guid.NewGuid().ToString())
                .With(x => x.Destination, Guid.NewGuid().ToString())
                .Create();

            var result = Mapper.Map<Flight>(entity);

            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(entity.Price, result.Price);
            Assert.AreEqual(entity.Transport, result.Transport);
            Assert.AreEqual(entity.Origin, result.Origin);
            Assert.AreEqual(entity.Destination, result.Destination);
        }

        [Test]
        public void ShouldMapApiClientApplicationRegistrationModel2()
        {
            var entity = Fixture.Create<FlightRepresentation>();

            var result = Mapper.Map<FlightModel>(entity);

            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(entity.DepartureStation, result.Origin);
            Assert.AreEqual(entity.ArrivalStation, result.Destination);
            Assert.AreEqual(entity.Price, result.Price);
            Assert.AreEqual(entity.FlightNumber, result.Transport.FlightNumber);
            Assert.AreEqual(entity.FlightCarrier, result.Transport.FlightCarrier);
        }
    }
}
