using Asg.Services.ApplicationFramework.Infrastructure.HttpClient;
using NewShore.Api.FunctionalTests.Infrastructure;
using NewShore.Domain.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewShore.Api.FunctionalTests.Controllers.Flights
{
    public class GetWithReturn : FunctionalTestBase
    {

        [SetUp]
        public void Setup()
        {
        }

        // curl -X GET "http://localhost:5000/api/v2/flights?Origin=we&Destination=we&MaxScales=5" -H  "accept: application/json"
        [Test]
        public async Task GivenId_ReturnsflightsModel()
        {
            //Arrange


            //Act
            var response = await Client.GetAsync($"/{ApiBasePath}/v2/flights?Origin=OCATA&Destination=BCN&MaxScales=4");

            //Assert
            response.EnsureSuccessStatusCode();

            var vm = response.Deserialize<IList<Journey>>();

            Assert.That(vm, Is.Not.Null);
            Assert.That(vm.Result.Count, Is.EqualTo(2));
            Assert.That(vm.Result[0].Origin, Is.EqualTo("OCATA"));
            Assert.That(vm.Result[0].Destination, Is.EqualTo("BCN"));
            Assert.That(vm.Result[0].Price, Is.EqualTo(1.1));
            Assert.That(vm.Result[0].Flights.Count, Is.EqualTo(1));

            Assert.That(vm.Result[1].Origin, Is.EqualTo("BCN"));
            Assert.That(vm.Result[1].Destination, Is.EqualTo("OCATA"));
            Assert.That(vm.Result[1].Price, Is.EqualTo(1.1));
            Assert.That(vm.Result[1].Flights.Count, Is.EqualTo(1));
        }
    }
}
