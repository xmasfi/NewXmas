using Microsoft.Extensions.Options;

namespace NewShore.Api.Client
{
    public class BaseClient
    {
        protected string BaseUrl { get; set; }

        protected BaseClient(IOptions<NewShoreConfiguration> configuration)
        {
            BaseUrl = configuration.Value.BaseUrl;
        }
    }
}