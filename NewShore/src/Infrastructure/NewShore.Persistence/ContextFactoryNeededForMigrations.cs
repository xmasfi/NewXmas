using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace NewShore.Persistence
{
    public class ContextFactoryNeededForMigrations : IDesignTimeDbContextFactory<NewShoreDbContext>
    {
        private const string ConnectionStringName = "NewShoreConnection";
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        public NewShoreDbContext CreateDbContext(string[] args)
        {
            var connectionString = GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder<NewShoreDbContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return new NewShoreDbContext(optionsBuilder.Options);
        }

        private string GetConnectionString()
        {
            var basePath = Directory.GetCurrentDirectory() + string.Format("{0}..{0}..{0}Presentation{0}NewShore.Api", Path.DirectorySeparatorChar);

            var environmentName = Environment.GetEnvironmentVariable(AspNetCoreEnvironment);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.Local.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString(ConnectionStringName);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"Connection string '{ConnectionStringName}' is null or empty.", nameof(connectionString));
            }

            Console.WriteLine($"DesignTimeDbContextFactoryBase.Create(string): Connection string: '{connectionString}'.");
            return connectionString;
        }
    }
}