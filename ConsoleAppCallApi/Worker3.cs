using Microsoft.Extensions.Hosting;
using Stef.AuditClient.MicrosoftIdentityClient;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppCallApi
{
    public class Worker3AzureIdentity : BackgroundService
    {
        private readonly IAuditClientMicrosoftIdentityClient _client;

        public Worker3AzureIdentity(IAuditClientMicrosoftIdentityClient client)
        {
            _client = client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _client.GetAsync(stoppingToken);

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}