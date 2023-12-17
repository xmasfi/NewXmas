using System.Reflection;
using Asg.Services.ApplicationFramework.Application.AutoMapper;
using Asg.Services.ApplicationFramework.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NewShore.Application.flights.Queries.Getflights;
using NewShore.Infrastructure.AutoMapper;

namespace NewShore.Api.Extensions.Configuration
{
    public static class ApplicationExtensions
    {
        /// <summary>
        ///     Adds the related Application Services.
        /// </summary>
        /// <remarks>
        ///     AutoMapper, MediatR and MediatR PipelineBehaviours
        /// </remarks>
        /// <param name="services">The services collection.</param>
        /// <returns></returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                .AddAutoMapper(
                    x => x.AddProfile<InfrastructureProfile>(), // Configure Infrastructure AutoMapper Profile
                    typeof(AutoMapperProfile).GetTypeInfo().Assembly)
                .AddMediatR(typeof(GetFlightQuery).GetTypeInfo().Assembly,typeof(RequestLogger<>).GetTypeInfo().Assembly)
                // Register PipelineBehaviours
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }
    }
}