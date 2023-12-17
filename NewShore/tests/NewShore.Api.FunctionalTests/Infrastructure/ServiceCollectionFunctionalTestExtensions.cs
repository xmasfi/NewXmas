using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NewShore.Persistence;

namespace NewShore.Api.FunctionalTests.Infrastructure
{
    public static class ServiceCollectionFunctionalTestExtensions
    {
                public static IServiceCollection Replace<TService, TImplementation>(this IServiceCollection services)where TService : class where TImplementation : class, TService
        {
            var descriptor = services.SingleOrDefault( d => d.ServiceType == typeof(TService));
            if (descriptor != null)
            {
                services.Remove(descriptor);
                services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), descriptor.Lifetime));
            }
            else
            {
                throw new Exception($"{typeof(TService).Name} not found!");
            }
            return services;
        }

        public static IServiceCollection Replace<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            var descriptor = services.SingleOrDefault( d => d.ServiceType == typeof(TService));
            if (descriptor != null)
            {
                services.Remove(descriptor);
                services.Add(new ServiceDescriptor(typeof(TService), implementationFactory, descriptor.Lifetime));
            }
            else
            {
                throw new Exception($"{typeof(TService).Name} not found!");
            }
            return services;
        }

        /// <summary>
        /// Replaces the database context using an in-memory database for testing.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IServiceCollection ReplaceDbContext<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            // Remove the app ApplicationDbContext registration.
            var descriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            else
            {
                throw new Exception($"{typeof(TContext).Name} not found!");
            }

            // Create a new service provider.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Add ApplicationDbContext using an in-memory database for testing.
            services.AddDbContext<TContext>(options =>
            {              
                options.UseInMemoryDatabase("InMemoryDbForTesting");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                options.UseInternalServiceProvider(serviceProvider);
            });

            return services;
        }

        public static IServiceCollection SeedNewShoreTestData(this IServiceCollection services)
        {
            // Build the service provider.
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database
            // context (NewShoreDbContext).
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<NewShoreDbContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<NewShoreDbContext>>();

                // Ensure the database is created.
                context.Database.EnsureCreated();

                try
                {
                    // Seed the database with test data.
                    DataBaseSeeder.Initialize(context);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                                        "database with test messages. Error: {Message}", ex.Message);
                }
            }

            return services;
        }
    }
}