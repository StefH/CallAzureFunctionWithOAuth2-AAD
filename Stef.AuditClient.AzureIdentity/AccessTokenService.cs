using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stef.AuditClient.AzureIdentity.Options;

namespace Stef.AuditClient.AzureIdentity
{
    internal class AccessTokenService : IAccessTokenService
    {
        private readonly Lazy<TokenRequestContext> _tokenRequestContext;
        private readonly Lazy<TokenCredential> _tokenCredential;
        private readonly ILogger<AccessTokenService> _logger;

        public AccessTokenService(ILogger<AccessTokenService> logger, IOptions<AuditClientMicrosoftIdentityClientOptions> options)
        {
            _logger = logger;

            _tokenRequestContext = new Lazy<TokenRequestContext>(() => new TokenRequestContext(new[] { $"{options.Value.Resource}/.default" }));
            _tokenCredential = new Lazy<TokenCredential>(() => CreateTokenCredential(options.Value));
        }

        public async Task<string> GetTokenAsync(CancellationToken cancellationToken)
        {
            var authenticationResult = await _tokenCredential.Value.GetTokenAsync(_tokenRequestContext.Value, cancellationToken);

            _logger.LogInformation("expire : " + authenticationResult.ExpiresOn.ToLocalTime());
            return authenticationResult.Token;
        }

        private TokenCredential CreateTokenCredential(AuditClientMicrosoftIdentityClientOptions optionsValue)
        {
            return new ChainedTokenCredential(
                new ManagedIdentityCredential(optionsValue.ClientId),
                new ClientSecretCredential(optionsValue.TenantId, optionsValue.ClientId, optionsValue.ClientSecret)
            );
        }
    }
}
