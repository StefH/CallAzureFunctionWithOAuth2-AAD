using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
//using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Stef.AuditClient.MicrosoftIdentityClient.Options;

namespace Stef.AuditClient.MicrosoftIdentityClient
{
    public class AuthenticationHttpMessageHandler : DelegatingHandler
    {
        private readonly ILogger<AuditClientMicrosoftIdentityClient> _logger;
        //private readonly ClientSecretCredential _clientSecretCredential;
        private readonly string[] _scopes;
        private IConfidentialClientApplication _confidentialClientApplication;

        public AuthenticationHttpMessageHandler(ILogger<AuditClientMicrosoftIdentityClient> logger, IOptions<AuditClientMicrosoftIdentityClientOptions> options)
        {
            _logger = logger;
            var auditClientOptions = options.Value;

            _scopes = new[] { $"{auditClientOptions.Resource}/.default" };

            //_clientSecretCredential = new ClientSecretCredential(auditClientOptions.TenantId, auditClientOptions.ClientId, auditClientOptions.ClientSecret);

            _confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(auditClientOptions.ClientId)
                .WithClientSecret(auditClientOptions.ClientSecret)
                .WithAuthority($"https://login.microsoftonline.com/{auditClientOptions.TenantId}/")
                .Build();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // var accessToken = (await _clientSecretCredential.GetTokenAsync(new TokenRequestContext(_scopes), cancellationToken)).Token;
            var accessToken = await GetAccessTokenAsync(cancellationToken);

            _logger.LogInformation("accessToken : " + accessToken);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            var authenticationResult = await _confidentialClientApplication.AcquireTokenForClient(_scopes).ExecuteAsync(cancellationToken);
            return authenticationResult.AccessToken;
        }
    }
}