using System.Threading.Tasks;
using Asg.Services.ApplicationFramework.Infrastructure.HttpClient;
using NUnit.Framework;
using NewShore.Api.FunctionalTests.Infrastructure;
using NewShore.Api.Models;
using NewShore.Domain.Entities;
using System.Collections.Generic;

namespace NewShore.Api.FunctionalTests.Controllers.flights
{
    public class Get : FunctionalTestBase
    {

        [SetUp]
        public void Setup()
        {
        }

        // curl -X GET "http://localhost:5000/api/v2/flights?Origin=we&Destination=we&MaxScales=5" -H  "accept: application/json"
        [Test]
        public async Task GivenId_flightsModel()
        {
            //Arrange

            //Act
            var response = await Client.GetAsync($"/{ApiBasePath}/v1/flights?Origin=OCATA&Destination=BCN&MaxScales=4");

            //Assert
            response.EnsureSuccessStatusCode();

            var vm = response.Deserialize<IList<Journey>>();

            Assert.That(vm, Is.Not.Null);
            Assert.That(vm.Result.Count, Is.EqualTo(1));
            Assert.That(vm.Result[0].Origin, Is.EqualTo("OCATA"));
            Assert.That(vm.Result[0].Destination, Is.EqualTo("BCN"));
            Assert.That(vm.Result[0].Price, Is.EqualTo(1.1));
            Assert.That(vm.Result[0].Flights.Count, Is.EqualTo(1));
        }
    }
}