using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewShore.Application.Interfaces;

namespace NewShore.Api.Extensions.Configuration
{
    public static class InfrastructureExtensions
    {
        /// <summary>
        /// Adds the related Infrastructure Services.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
            // Register here your Infrastructure Services
                   .AddTransient<INewShoreAIRService, Infrastructure.Services.NewShoreAIRService>()
                   .AddHttpClientServices(configuration);
        }
    }
}