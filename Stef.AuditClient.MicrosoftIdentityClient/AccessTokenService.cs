using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Options;
using Stef.AuditClient.MicrosoftIdentityClient.Options;

namespace Stef.AuditClient.MicrosoftIdentityClient
{
    internal class AccessTokenService : IAccessTokenService
    {
        private readonly Lazy<TokenRequestContext> _TokenRequestContext;
        private readonly Lazy<TokenCredential> _tc;

        public AccessTokenService(IOptions<AuditClientMicrosoftIdentityClientOptions> options)
        {
            var optionsValue = options.Value;
            _TokenRequestContext = new Lazy<TokenRequestContext>(() => new TokenRequestContext(new[] { $"{optionsValue.Resource}/.default" }));
            //_scopes = new[] { $"{options.Value.Resource}/.default" };

            _tc = new Lazy<TokenCredential>(() =>
               new ClientSecretCredential(optionsValue.TenantId, optionsValue.ClientId, optionsValue.ClientSecret));
        }

        public async Task<string> GetAccessToken(CancellationToken cancellationToken)
        {
            //var tokenRequestContext = new TokenRequestContext(_scopes);
            var authenticationResult = await _tc.Value.GetTokenAsync(_TokenRequestContext.Value, cancellationToken);

            //_logger.LogInformation("expire : " + authenticationResult.ExpiresOn.ToLocalTime());
            return authenticationResult.Token;
        }
    }
}
