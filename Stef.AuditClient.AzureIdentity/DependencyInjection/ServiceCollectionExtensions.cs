using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Stef.AuditClient.AzureIdentity;
using Stef.AuditClient.AzureIdentity.Constants;
using Stef.AuditClient.AzureIdentity.Options;
using Stef.AuditClient.AzureIdentity.RetryPolicies;
using Stef.AuditClient.AzureIdentity.Validation;
using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        [PublicAPI]
        public static IServiceCollection AddAuditClientAzureIdentity(this IServiceCollection services, IConfigurationSection section)
        {
            Guard.NotNull(services, nameof(services));

            var auditClientAzureIdentityOptions = new AuditClientMicrosoftIdentityClientOptions();
            section.Bind(auditClientAzureIdentityOptions);

            return services.AddAuditClientAzureIdentity(auditClientAzureIdentityOptions);
        }

        [PublicAPI]
        public static IServiceCollection AddAuditClientAzureIdentity(this IServiceCollection services, Action<AuditClientMicrosoftIdentityClientOptions> configureAction)
        {
            Guard.NotNull(services, nameof(services));
            Guard.NotNull(configureAction, nameof(configureAction));

            var options = new AuditClientMicrosoftIdentityClientOptions();
            configureAction(options);

            return services.AddAuditClientAzureIdentity(options);
        }

        private static IServiceCollection AddAuditClientAzureIdentity(this IServiceCollection services, AuditClientMicrosoftIdentityClientOptions options)
        {
            if (string.IsNullOrEmpty(options.HttpClientName))
            {
                options.HttpClientName = AuditClientAzureIdentityConstants.Name;
            }

            services
                //.AddSingleton(ConfidentialClientApplicationBuilder
                //    .Create(options.ClientId)
                //    .WithClientSecret(options.ClientSecret)
                //    .WithAuthority($"https://login.microsoftonline.com/{options.TenantId}/")
                //    .Build()
                //)

                .AddTransient<AuthenticationHttpMessageHandler>()
                .AddHttpClient<IAuditClientAzureIdentity, AuditClientAzureIdentity>(options.HttpClientName, httpClient =>
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
            //            serviceProvider.GetService<ILogger<AuditClientAzureIdentity>>()
            //                .LogWarning("Delaying for {delay}ms, then making retry {retry}.", timespan.TotalMilliseconds, retryAttempt);
            //        }
            //    ));

            services.AddHttpClient("google", httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://www.google.com");
            });

            services.AddSingleton(Options.Options.Create(options));

            services.AddSingleton<IAccessTokenService, AccessTokenService>();
            //services.AddSingleton<IAuditClientAzureIdentity, AuditClientAzureIdentity>();

            return services;
        }
    }
}