using System;
using AuditClient.Options;
using IdentityModel.Client;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Stef.AuditClient;
using Stef.AuditClient.Constants;
using Stef.AuditClient.Validation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        [PublicAPI]
        public static IServiceCollection AddAuditClient(this IServiceCollection services, IConfigurationSection section)
        {
            Guard.NotNull(services, nameof(services));

            var options = new AuditClientOptions();
            section.Bind(options);

            return services.AddAuditClient(options);
        }

        [PublicAPI]
        public static IServiceCollection AddAuditClient(this IServiceCollection services, Action<AuditClientOptions> configureAction)
        {
            Guard.NotNull(services, nameof(services));
            Guard.NotNull(configureAction, nameof(configureAction));

            var options = new AuditClientOptions();
            configureAction(options);

            return services.AddAuditClient(options);
        }

        private static IServiceCollection AddAuditClient(this IServiceCollection services, AuditClientOptions options)
        {
            services.AddAccessTokenManagement(accessTokenManagementOptions =>
            {
                accessTokenManagementOptions.Client.Clients.Add("AzureAD", new ClientCredentialsTokenRequest
                {
                    Address = $"https://login.microsoftonline.com/{options.TenantId}/oauth2/token",
                    ClientId = options.ClientId,
                    ClientSecret = options.ClientSecret,
                    GrantType = "client_credentials",
                    Parameters =
                    {
                        { "resource", options.Resource }
                    }
                });
            });

            services.AddClientAccessTokenClient(AuditClientConstants.Name, configureClient: client =>
            {
                client.BaseAddress = new Uri(options.BaseAddress);
            });

            services.AddSingleton<IAuditClient, Stef.AuditClient.AuditClient>();

            return services;
        }
    }
}