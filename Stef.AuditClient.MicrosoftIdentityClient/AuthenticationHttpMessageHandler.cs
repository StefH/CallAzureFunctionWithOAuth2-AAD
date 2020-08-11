using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Stef.AuditClient.MicrosoftIdentityClient
{
    internal class AuthenticationHttpMessageHandler : DelegatingHandler
    {
        private readonly ILogger<AuthenticationHttpMessageHandler> _logger;
        private readonly IAccessTokenService _tokenService;

        public AuthenticationHttpMessageHandler(ILogger<AuthenticationHttpMessageHandler> logger, IAccessTokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _tokenService.GetTokenAsync(cancellationToken); //GetAccessTokenViaTokenCredentialAsync(cancellationToken);

            _logger.LogInformation("accessToken : " + accessToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}