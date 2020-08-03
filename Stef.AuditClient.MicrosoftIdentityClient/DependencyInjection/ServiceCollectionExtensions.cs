﻿using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Stef.AuditClient.MicrosoftIdentityClient;
using Stef.AuditClient.MicrosoftIdentityClient.Constants;
using Stef.AuditClient.MicrosoftIdentityClient.Options;
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

                .AddHttpClient(options.HttpClientName, httpClient =>
                {
                    httpClient.BaseAddress = options.BaseAddress;
                })

                .AddHttpMessageHandler<AuthenticationHttpMessageHandler>();

            services.AddSingleton(Options.Options.Create(options));

            services.AddSingleton<IAuditClientMicrosoftIdentityClient, AuditClientMicrosoftIdentityClient>();

            return services;
        }
    }
}