using Asg.Services.ApplicationFramework.Presentation.HttpClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Asg.Services.ApplicationFramework.Presentation.Extensions;
using Asg.Services.ApplicationFramework.Presentation.Polly;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using NewShore.Api.Config.HttpClientServices;
using NewShoreAIR.Api.Client;
using NewShoreAIR.Api.Client.Contracts;

namespace NewShore.Api.Extensions.Configuration
{
    public static class HttpClientServicesExtensions
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpClientLogErrorExtensions();

            var timeout = configuration.GetValue<int>("HttpClientServices:Timeout");

            services.AddDefaultDelegatingHandlers();

            // NewShoreAIR
            services.Configure<NewShoreAIRHttpClientConfig>(configuration.GetSection(NewShoreAIRHttpClientConfig.Section));
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<NewShoreAIRHttpClientConfig>, NewShoreAIRHttpClientConfigValidation>());

            // Http delegation order matters!
            services.AddHttpClient<INewShoreAIRClient, NewShoreAIRClient>((sp, x) =>
                    x.BaseAddress = new Uri(sp.GetRequiredService<IOptions<NewShoreAIRHttpClientConfig>>().Value.BaseUrl))
                .AddHttpMessageHandler<HttpClientProcessErrorDelegatingHandler>()
                .AddPolicyHandler((serviceProvider, _) => RetryPolicy.GetPolicyWithJitterStrategy(serviceProvider, 3))
                .AddPolicyHandler((serviceProvider, _) => CircuitBreakerPolicy.GetCircuitBreakerPolicy(serviceProvider))
                .AddPolicyHandler((serviceProvider, _) => TimeoutPolicy.GetOptimisticTimeoutPolicy(serviceProvider, timeout));

            return services;
        }
    }
}
