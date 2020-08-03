using System.Threading;
using System.Threading.Tasks;
using Stef.AuditClient.MicrosoftIdentityClient;

namespace ConsoleAppNetFrameworkOld461
{
    public class Worker
    {
        private readonly IAuditClientMicrosoftIdentityClient _client;

        public Worker(IAuditClientMicrosoftIdentityClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _client.GetAsync(stoppingToken);

                await Task.Delay(6101, stoppingToken);
            }
        }
    }
}