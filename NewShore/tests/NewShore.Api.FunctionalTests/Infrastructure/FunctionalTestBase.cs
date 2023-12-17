using System.Net.Http;
using System.Text;
using Asg.Services.ApplicationFramework.Presentation.Web;
using AutoFixture;
using Newtonsoft.Json;
using NUnit.Framework;

namespace NewShore.Api.FunctionalTests.Infrastructure
{
    [TestFixture]
    public class FunctionalTestBase
    {
        protected const string ApiBasePath = ApiConstants.ApiBasePath;

        private CustomWebApplicationFactory<Startup> _factory;
        protected HttpClient Client;
        protected Fixture Fixture;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
            Client = _factory.CreateClient();
            Fixture = new Fixture();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Client.Dispose();
            _factory.Dispose();
        }

        protected static StringContent GetRequestContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}