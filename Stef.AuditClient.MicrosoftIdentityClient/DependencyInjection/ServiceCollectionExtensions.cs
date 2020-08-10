using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Polly;
using Polly.Extensions.Http;
using Stef.AuditClient.MicrosoftIdentityClient;
using Stef.AuditClient.MicrosoftIdentityClient.Constants;
using Stef.AuditClient.MicrosoftIdentityClient.Options;
using Stef.AuditClient.MicrosoftIdentityClient.RetryPolicies;
using Stef.AuditClient.MicrosoftIdentityClient.Validation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        [PublicAPI]
        public static IServiceCollection AddAuditClientMicrosoftIdentity(this IServiceCollection services, IConfigurationSection section)
        {
            Guard.NotNull(services, nameof(services));

            var auditClientAzureIdentityOptions = new AuditClientMicrosoftIdentityClientOptions();
            section.Bind(auditClientAzureIdentityOptions);

            return services.AddAuditClientMicrosoftIdentity(auditClientAzureIdentityOptions);
        }

        [PublicAPI]
        public static IServiceCollection AddAuditClientMicrosoftIdentity(this IServiceCollection services, Action<AuditClientMicrosoftIdentityClientOptions> configureAction)
        {
            Guard.NotNull(services, nameof(services));
            Guard.NotNull(configureAction, nameof(configureAction));

            var options = new AuditClientMicrosoftIdentityClientOptions();
            configureAction(options);

            return services.AddAuditClientMicrosoftIdentity(options);
        }

        private static IServiceCollection AddAuditClientMicrosoftIdentity(this IServiceCollection services, AuditClientMicrosoftIdentityClientOptions options)
        {
            if (string.IsNullOrEmpty(options.HttpClientName))
            {
                options.HttpClientName = AuditClientMicrosoftIdentityClientConstants.Name;
            }

            services
                .AddSingleton(ConfidentialClientApplicationBuilder
                    .Create(options.ClientId)
                    .WithClientSecret(options.ClientSecret)
                    .WithAuthority($"https://login.microsoftonline.com/{options.TenantId}/")
                    .Build()
                )

                .AddTransient<AuthenticationHttpMessageHandler>()
                .AddHttpClient<IAuditClientMicrosoftIdentityClient, AuditClientMicrosoftIdentityClient>(options.HttpClientName, httpClient =>
                {
                    httpClient.BaseAddress = options.BaseAddress;
                })
                .AddHttpMessageHandler<AuthenticationHttpMessageHandler>()
                .AddPolicyHandler((serviceProvider, request) => AuditClientPolicies.RetryPolicy(serviceProvider));
            //.AddPolicyHandler((serviceProvider, request) =>
            //HttpPolicyExtensions.HandleTransientHttpError()
            //    .WaitAndRetryAsync(3,
            //        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            //        onRetry: (outcome, timespan, retryAttempt, context) =>
            //        {
            //            serviceProvider.GetService<ILogger<AuditClientMicrosoftIdentityClient>>()
            //                .LogWarning("Delaying for {delay}ms, then making retry {retry}.", timespan.TotalMilliseconds, retryAttempt);
            //        }
            //    ));

            services.AddHttpClient("google", httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://www.google.com");
            });

            services.AddSingleton(Options.Options.Create(options));

            services.AddSingleton<IAccessTokenService, AccessTokenService>();
            //services.AddSingleton<IAuditClientMicrosoftIdentityClient, AuditClientMicrosoftIdentityClient>();

            return services;
        }
    }
}