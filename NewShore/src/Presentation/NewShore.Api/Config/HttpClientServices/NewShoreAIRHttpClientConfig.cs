using Asg.Services.ApplicationFramework.Presentation.Config.HttpClientServices;

namespace NewShore.Api.Config.HttpClientServices
{
    public class NewShoreAIRHttpClientConfig : IHttpClientConfig
    {
        public const string Section = "HttpClientServices:NewShoreAIR";

        public string BaseUrl { get; set; }
    }
}
