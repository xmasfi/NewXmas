using Moq;
using NewShore.Application.flights.Queries.Getflights;
using NewShore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NewShore.Application.IntegrationTests.Builders
{
    
    namespace CloudAdministrationTool.Application.IntegrationTests.Builders
    {
        public class NewShoreAIRServiceMockBuilder
        {
            private readonly Mock<INewShoreAIRService> _newShoreAIRServiceMock = new Mock<INewShoreAIRService>();


            public NewShoreAIRServiceMockBuilder ComplexScenario()
            {

                _newShoreAIRServiceMock
                    .Setup(x => x.GetFlights(CancellationToken.None))
                    .ReturnsAsync(new List<FlightModel>
                    {
                        new FlightModel()
                        {
                            Origin = "BCN",
                            Destination = "MAD",
                            Price = 10.0,
                            Transport = new Domain.ValueObjects.Transport("AAA", "ZZZ")
                        }
                    });

                return this;
            }

                public INewShoreAIRService Build()
            {
                return _newShoreAIRServiceMock.Object;
            }

            public Mock<INewShoreAIRService> GetMock()
            {
                return _newShoreAIRServiceMock;
            }

        }
    }
}
