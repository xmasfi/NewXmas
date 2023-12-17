using FluentValidation.Validators.UnitTestExtension.Composer;
using FluentValidation.Validators.UnitTestExtension.Core;
using FluentValidation.Validators;
using NewShore.Application.Flights.Queries.GetFlights;
using NUnit.Framework;
using NewShore.Application.flights.Queries.Getflights;

namespace NewShore.Application.IntegrationTests.Flights.Queries.GetFlights
{
    public class GetFlightQueryHandlerValidationTests
    {
        private GetFlightQueryHandlerValidation _validator;

        [SetUp]
        public void Setup()
        {

            _validator = new GetFlightQueryHandlerValidation();
        }

        [Test]
        public void ShouldHaveConfiguredValidationRulesCorrectlyForParementers()
        {
            _validator.ShouldHaveRules(query => query.Origin,
                BaseVerifiersSetComposer.Build()
                    .AddPropertyValidatorVerifier<NotEmptyValidator>()
                    .AddPropertyValidatorVerifier<LengthValidator>()
                    .Create());

            _validator.ShouldHaveRules(query => query.Destination,
                BaseVerifiersSetComposer.Build()
                    .AddPropertyValidatorVerifier<NotEmptyValidator>()
                    .AddPropertyValidatorVerifier<LengthValidator>()
                    .Create());

            _validator.ShouldHaveRules(query => query.MaxScales,
                BaseVerifiersSetComposer.Build()
                    .AddPropertyValidatorVerifier<GreaterThanOrEqualValidator>()
                    .AddPropertyValidatorVerifier<LessThanOrEqualValidator>()
                    .Create());
        }

        [Test]
        [TestCase("BCN", "MAD", 5)]
        public void ShouldBeValidIfCorrectParameters(string destination, string origin, int maxScales)
        {
            // Arrange
            var command = new GetFlightQuery
            {
                Destination = destination,
                Origin = origin,
                MaxScales = maxScales  
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.That(result.IsValid, Is.True);
        }


        [Test]
        [TestCase("BCN", "MAD", 0)]
        [TestCase("BCN", "MAD", -11)]
        [TestCase("BCN", "MAD", 11)]
        [TestCase("BCN", "BCN", 5)]
        public void ShouldBeInValidIfIncorrectParameters(string destination, string origin, int maxScales)
        {
            // Arrange
            var command = new GetFlightQuery
            {
                Destination = destination,
                Origin = origin,
                MaxScales = maxScales
            };

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.That(result.IsValid, Is.False);
        }
    }
}
