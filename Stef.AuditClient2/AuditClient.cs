using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stef.AuditClient.Constants;

namespace Stef.AuditClient
{
    public class AuditClient : IAuditClient
    {
        private readonly ILogger<AuditClient> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public AuditClient(ILogger<AuditClient> logger, IHttpClientFactory factory)
        {
            _logger = logger;
            _clientFactory = factory;
        }

        public async Task<string> GetAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new Exception();
            }

            _logger.LogInformation("GetAsync at: {time}", DateTimeOffset.Now);

            var client = _clientFactory.CreateClient(AuditClientConstants.Name);
            var response = await client.GetAsync("weatherforecast", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API response: {response}", content);
                return content;
            }
            else
            {
                _logger.LogError("API returned: {statusCode}", response.StatusCode);
                return null;
            }
        }
    }
}
