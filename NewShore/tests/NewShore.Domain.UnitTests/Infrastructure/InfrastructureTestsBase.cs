using AutoFixture;
using AutoMapper;
using NUnit.Framework;

namespace NewShore.Domain.UnitTests.Infrastructure
{
    public class InfrastructureTestsBase<TProfile> where TProfile : Profile, new()
    {
        protected IMapper Mapper;
        private IConfigurationProvider _configurationProvider;

        protected Fixture Fixture;

        [OneTimeSetUp]
        public void Setup()
        {
            _configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TProfile>();
            });

            _configurationProvider.AssertConfigurationIsValid();

            Mapper = _configurationProvider.CreateMapper();

            Fixture = new Fixture();
        }
    }
}
