using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NewShore.Persistence;
using AutoMapper;
using NewShore.Infrastructure.AutoMapper;
using Asg.Services.ApplicationFramework.Application.AutoMapper;

namespace NewShore.Application.IntegrationTests.Infrastructure
{
    [Parallelizable]
    [TestFixture]
    public class IntegrationTestBase
    {
        protected IMapper Mapper;
        private IConfigurationProvider _configurationProvider;

        [OneTimeSetUp]
        public void Setup()
        {
            _configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<InfrastructureProfile>();
                cfg.AddProfile(new AutoMapperProfile());
            });

            Mapper = _configurationProvider.CreateMapper();
        }

        public NewShoreDbContext GetDbContextSqlLite()
        {
            var builder = new DbContextOptionsBuilder<NewShoreDbContext>();
            builder.UseSqlite("DataSource=:memory:", x => { });
            builder.EnableSensitiveDataLogging();

            var dbContext = new NewShoreDbContext(builder.Options);
            // SQLite needs to open connection to the DB.
            // Not required for in-memory-database.
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        public NewShoreDbContext GetDbContextSqlLiteInMemory()
        {
            var builder = new DbContextOptionsBuilder<NewShoreDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            var dbContext = new NewShoreDbContext(builder.Options);

            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }
}