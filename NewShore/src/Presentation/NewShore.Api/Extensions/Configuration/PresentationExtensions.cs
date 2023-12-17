using Asg.Services.ApplicationFramework.Presentation.ErrorHandling;
using Asg.Services.ApplicationFramework.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewShore.Application.Flights.Queries.GetFlights;

namespace NewShore.Api.Extensions.Configuration
{
    public static class PresentationExtensions
    {
        /// <summary>
        /// Adds the Presentation related Services.
        /// </summary>
        /// <remarks>
        /// HealthChecks, OpenApi and MVC Controllers
        /// </remarks>
        /// <param name="services">The services collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddRouting(options => options.LowercaseUrls = true)
                .Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; })
                .AddCustomHealthChecks(configuration)
                .AddCustomOpenApi(configuration)
                .AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
                    .ConfigureMvc(typeof(GetFlightQueryHandlerValidation))
                    .Services;
        }
    }
}