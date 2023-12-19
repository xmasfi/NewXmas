using Asg.Services.ApplicationFramework.Presentation.Extensions;
using Asg.Services.ApplicationFramework.Presentation.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewShore.Api.Extensions.Configuration;

[assembly: ApiConventionType(typeof(ApiConventions))]

namespace NewShore.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddInfrastructure(Configuration)
                .AddPersistence(Configuration)
                .AddApplication()
                .AddPresentation(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            app
                .UseIf(env.IsDevelopment(), appBuilder => appBuilder.UseDeveloperExceptionPage())
                .UseApplicationFrameworkErrorHandling()
                .UseCustomOpenApi(Configuration, ApiConstants.ApiBasePath, apiVersionDescriptionProvider)
                .UsePathBase(ApiConstants.ApiBasePath)
                .UseRouting()
                .UseAuthorization()
                .UseCustomEndPoints();
        }
    }
}