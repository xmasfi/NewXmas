using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NewShore.Api.Config;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NewShore.Api.Extensions.Configuration
{
    public static class OpenApiExtensions
    {
        private const string NewShoreApi = "NewShore API";
        
        public static IServiceCollection AddCustomOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(2, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(2, 0);
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            services.AddSwaggerGen(o =>
            {
                o.IncludeXmlComments(xmlFilePath);
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
            var versions = new[]
            {
                // Here you can control the minor version of each supported major version
                new Version(2, 0),
                new Version(1, 0)
            };

            foreach (var version in versions)
            {
                services.AddOpenApiDocument(options =>
                {
                    options.Title = "NewShore Service";
                    options.Description = "Manages products and their metadata.";

                    options.DocumentName = "v" + version.Major;
                    options.ApiGroupNames = new string[] { "v" + version.Major };
                    options.Version = version.Major + "." + version.Minor;

                    // Patch document for Azure API Management
                    options.AllowReferencesWithProperties = true;
                    options.PostProcess = document =>
                    {
                        var prefix = "/api/v" + version.Major;
                        foreach (var pair in document.Paths.ToArray())
                        {
                            document.Paths.Remove(pair.Key);
                            document.Paths[pair.Key.Substring(prefix.Length)] = pair.Value;
                        }
                    };
                });
            }

            return services;
        }

        public static IApplicationBuilder UseCustomOpenApi(this IApplicationBuilder app, IConfiguration configuration,  string basePath, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            return app
                .UseOpenApi(options =>
                {
                    options.PostProcess = (document, request) =>
                    {
                        // Patch server URL for Swagger UI
                        var prefix = "/api/v" + document.Info.Version.Split('.')[0];
                        document.Servers.First().Url += prefix;
                    };
                })
                .UseSwagger(options =>
                {
                    options.PreSerializeFilters.Add((swagger, req) =>
                    {
                        swagger.Servers = new List<OpenApiServer>() { new OpenApiServer() { Url = $"https://{req.Host}" } };
                    });
                })
                .UseSwaggerUI(options =>
                {
                    foreach (var desc in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"../swagger/{desc.GroupName}/swagger.json", desc.ApiVersion.ToString());
                        options.DefaultModelsExpandDepth(-1);
                        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    }
                })          
                .UseSwaggerUi3(settings =>
                {
                    settings.Path = basePath;
                });

             }
        }
    }