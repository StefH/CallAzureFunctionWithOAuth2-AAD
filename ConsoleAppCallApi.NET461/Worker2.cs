using System.Threading;
using System.Threading.Tasks;
using Stef.AuditClient.AzureIdentity;

namespace ConsoleAppCallApi.NET461
{
    public class Worker2 //: BackgroundService
    {
        private readonly IAuditClientAzureIdentity _client;

        public Worker2(IAuditClientAzureIdentity client)
        {
            _client = client;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _client.GetAsync(stoppingToken);

                await Task.Delay(5101, stoppingToken);
            }
        }
    }
}