using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Stef.AuditClient.MicrosoftIdentityClient.Options;

namespace Stef.AuditClient.MicrosoftIdentityClient
{
    public class AuthenticationHttpMessageHandler : DelegatingHandler
    {
        private readonly ILogger<AuditClientMicrosoftIdentityClient> _logger;
        private readonly string[] _scopes;
        private readonly IConfidentialClientApplication _confidentialClientApplication;
        private readonly TokenCredential tc;

        public AuthenticationHttpMessageHandler(
            ILogger<AuditClientMicrosoftIdentityClient> logger,
            IOptions<AuditClientMicrosoftIdentityClientOptions> options,
            IConfidentialClientApplication confidentialClientApplication
            )
        {
            _logger = logger;
            _confidentialClientApplication = confidentialClientApplication;

            var auditClientOptions = options.Value;

            _scopes = new[] { $"{auditClientOptions.Resource}/.default" };

            tc = new ClientSecretCredential(auditClientOptions.TenantId, auditClientOptions.ClientId, auditClientOptions.ClientSecret);

            //_confidentialClientApplication = ConfidentialClientApplicationBuilder
            //    .Create(auditClientOptions.ClientId)
            //    .WithClientSecret(auditClientOptions.ClientSecret)
            //    .WithAuthority($"https://login.microsoftonline.com/{auditClientOptions.TenantId}/")
            //    .Build();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await GetAccessTokenViaTokenCredentialAsync(cancellationToken);

            _logger.LogInformation("accessToken : " + accessToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private async Task<string> GetAccessTokenViaConfidentialClientApplicationAsync(CancellationToken cancellationToken)
        {
            var authenticationResult = await _confidentialClientApplication.AcquireTokenForClient(_scopes).ExecuteAsync(cancellationToken);
            return authenticationResult.AccessToken;
        }

        private async Task<string> GetAccessTokenViaTokenCredentialAsync(CancellationToken cancellationToken)
        {
            var tokenRequestContext = new TokenRequestContext(_scopes);
            var authenticationResult = await tc.GetTokenAsync(tokenRequestContext, cancellationToken);

            _logger.LogInformation("expire : " + authenticationResult.ExpiresOn.ToLocalTime());
            return authenticationResult.Token;
        }
    }
}