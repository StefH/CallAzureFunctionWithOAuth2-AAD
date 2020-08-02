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
        private readonly IOptions<AuditClientMicrosoftIdentityClientOptions> _options;
        private readonly IHttpClientFactory _clientFactory;
        private IConfidentialClientApplication _app;
        //private readonly ClientSecretCredential _clientSecretCredential;

        public AuditClientMicrosoftIdentityClient(ILogger<AuditClientMicrosoftIdentityClient> logger, IOptions<AuditClientMicrosoftIdentityClientOptions> options, IHttpClientFactory factory)
        {
            _logger = logger;
            _options = options;
            _clientFactory = factory;

           

           

            //_clientSecretCredential = new ClientSecretCredential(auditClientOptions.TenantId, auditClientOptions.ClientId, auditClientOptions.ClientSecret);
        }

        public async Task<string> GetAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new Exception();
            }

            _logger.LogInformation("GetAsync at: {time}", DateTimeOffset.Now);

            var client = _clientFactory.CreateClient(AuditClientMicrosoftIdentityClientConstants.Name);

            string to = "";
            try
            {
                // var s = new[] { "API.Access" };
                var s = new[]  {$"{_options.Value.Resource}/.default"};
                //var t = await _clientSecretCredential.GetTokenAsync(new TokenRequestContext(s), cancellationToken);
                //to = t.Token;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //client.BaseAddress = new Uri(_options.Value.BaseAddress);
           // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", to);

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
