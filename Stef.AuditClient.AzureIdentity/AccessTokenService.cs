using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stef.AuditClient.AzureIdentity.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stef.AuditClient.AzureIdentity
{
    internal class AccessTokenService : IAccessTokenService
    {
        private const string Key = "StefAccessTokenService_AccessToken";

        private readonly Lazy<TokenRequestContext> _tokenRequestContext;
        private readonly Lazy<TokenCredential> _tokenCredential;
        private readonly ILogger<AccessTokenService> _logger;
        private readonly IMemoryCache _cache;

        public AccessTokenService(ILogger<AccessTokenService> logger, IOptions<AuditClientMicrosoftIdentityClientOptions> options, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;

            _tokenRequestContext = new Lazy<TokenRequestContext>(() => new TokenRequestContext(new[] { $"{options.Value.Resource}/.default" }));
            _tokenCredential = new Lazy<TokenCredential>(() => CreateTokenCredential(options.Value));
        }

        public async Task<string> GetTokenAsync(CancellationToken cancellationToken)
        {
            var cachedAccessToken = await
                _cache.GetOrCreateAsync(Key, async entry =>
                {
                    _logger.LogInformation("getting new token");
                    var accessToken = await _tokenCredential.Value.GetTokenAsync(_tokenRequestContext.Value, cancellationToken).ConfigureAwait(false);

                    entry.AbsoluteExpiration = accessToken.ExpiresOn;
                    return accessToken;
                }).ConfigureAwait(false);

            _logger.LogInformation("token  : " + cachedAccessToken.Token);
            _logger.LogInformation("expire : " + cachedAccessToken.ExpiresOn.ToLocalTime());

            return cachedAccessToken.Token;
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
