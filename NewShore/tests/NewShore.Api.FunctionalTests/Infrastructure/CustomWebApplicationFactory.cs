using NewShore.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using NewShore.Api.FunctionalTests.Builders;

namespace NewShore.Api.FunctionalTests.Infrastructure
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Replace DbContext using an in-memory one with test data
                services
                    .Replace(_ => new NewShoreAIRServiceMockBuilder().Simple().Build())
                    .ReplaceDbContext<NewShoreDbContext>()
                    .SeedNewShoreTestData();

                // Register here the Test Services (Fakes, Mocks, others)
            });
        }
    }
}