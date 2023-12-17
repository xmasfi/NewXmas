using Moq;
using NewShore.Application.flights.Queries.Getflights;
using NewShore.Application.Interfaces;
using System.Collections.Generic;
using System.Threading;

namespace NewShore.Api.FunctionalTests.Builders
{
    public class NewShoreAIRServiceMockBuilder
    {
        private readonly Mock<INewShoreAIRService> _newShoreAIRServiceMock = new();

        public NewShoreAIRServiceMockBuilder Simple()
        {
            _newShoreAIRServiceMock
                .Setup(x => x.GetFlights(CancellationToken.None))
                .ReturnsAsync(new List<FlightModel>() 
                {
                    new FlightModel()
                    {
                        Origin = "OCATA",
                        Destination = "BCN",
                        Price = 1.1,
                        Transport = new Domain.ValueObjects.Transport("renfe", "cercanias")
                    }
                });

            return this;
        }
            public INewShoreAIRService Build()
        {
            return _newShoreAIRServiceMock.Object;
        }

    }
}
