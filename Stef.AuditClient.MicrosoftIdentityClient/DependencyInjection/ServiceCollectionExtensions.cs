//using IdentityModel.Client;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Stef.AuditClient.MicrosoftIdentityClient;
using Stef.AuditClient.MicrosoftIdentityClient.Constants;
using Stef.AuditClient.MicrosoftIdentityClient.Options;
using Stef.AuditClient.MicrosoftIdentityClient.Validation;
using System;

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

        private static IServiceCollection AddAuditClientMicrosoftIdentity(this IServiceCollection services, AuditClientMicrosoftIdentityClientOptions microsoftIdentityClientOptions)
        {
            services
                .AddTransient<AuthenticationHttpMessageHandler>()
                .AddHttpClient(AuditClientMicrosoftIdentityClientConstants.Name, c =>
                {
                    c.BaseAddress = new Uri(microsoftIdentityClientOptions.BaseAddress);
                })
                .AddHttpMessageHandler<AuthenticationHttpMessageHandler>();

            services.AddSingleton(Options.Options.Create(microsoftIdentityClientOptions));

            services.AddSingleton<IAuditClientMicrosoftIdentityClient, AuditClientMicrosoftIdentityClient>();

            return services;
        }
    }
}