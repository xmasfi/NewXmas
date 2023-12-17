using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NewShore.Api.Extensions.Configuration
{
    public static class OpenApiExtensions
    {
        private const string NewShoreApi = "NewShore API";

        public static IServiceCollection AddCustomOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddOpenApiDocument(settings =>
            {
                settings.Title = NewShoreApi;
                settings.Description = NewShoreApi;
            });
        }

        public static IApplicationBuilder UseCustomOpenApi(this IApplicationBuilder app, IConfiguration configuration,  string basePath)
        {
            return app
                .UseOpenApi(settings => { settings.PostProcess = (doc, req) => { doc.BasePath = basePath; }; })
                .UseSwaggerUi3(settings =>
                {
                    settings.Path = basePath;
                });
        }
    }
}