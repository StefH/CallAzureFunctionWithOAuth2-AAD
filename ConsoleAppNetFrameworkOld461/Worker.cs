using System.Threading;
using System.Threading.Tasks;
using Stef.AuditClient.AzureIdentity;

namespace ConsoleAppNetFrameworkOld461
{
    public class Worker
    {
        private readonly IAuditClientAzureIdentity _client;

        public Worker(IAuditClientAzureIdentity client)
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