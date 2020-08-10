using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
//using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Stef.AuditClient.MicrosoftIdentityClient.Constants;
using Stef.AuditClient.MicrosoftIdentityClient.Options;

namespace Stef.AuditClient.MicrosoftIdentityClient
{
    public class AuditClientMicrosoftIdentityClient : IAuditClientMicrosoftIdentityClient
    {
        private readonly ILogger<AuditClientMicrosoftIdentityClient> _logger;
        private readonly AuditClientMicrosoftIdentityClientOptions _options;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;

        public AuditClientMicrosoftIdentityClient(
            ILogger<AuditClientMicrosoftIdentityClient> logger,
            IOptions<AuditClientMicrosoftIdentityClientOptions> options,
            IHttpClientFactory factory, HttpClient client)
        {
            _logger = logger;
            _options = options.Value;
            _clientFactory = factory;
            _client = client;
        }

        public async Task<string> GetAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new Exception();
            }

            _logger.LogInformation("GetAsync at: {time}", DateTimeOffset.Now);

            var client = _clientFactory.CreateClient(_options.HttpClientName);
            var response = await client.GetAsync("weatherforecast", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API response: {response}", content);
                return content;
            }

            _logger.LogError("API returned: {statusCode}", response.StatusCode);
            return null;
        }
    }


}
