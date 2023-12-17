using Asg.Services.ApplicationFramework.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewShore.Application.Interfaces;
using NewShore.Persistence;

namespace NewShore.Api.Extensions.Configuration
{
    public static class PersistenceExtensions
    {
        /// <summary>
        /// Adds the Persistence related Services.
        /// </summary>
        /// <remarks>
        /// NewShoreDbContext
        /// </remarks>
        /// <param name="services">The services collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext<NewShoreDbContext>(optionsAction =>
                    optionsAction.UseNpgsql(configuration.GetConnectionString("NewShoreConnection")))
                .AddScoped<INewShoreDbContext>(provider => provider.GetService<NewShoreDbContext>());
;
        }
    }
}