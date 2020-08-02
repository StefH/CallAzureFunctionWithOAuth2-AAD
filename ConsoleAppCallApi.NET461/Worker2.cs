using System.Threading;
using System.Threading.Tasks;
using Stef.AuditClient;

namespace ConsoleAppCallApi.NET461
{
    public class Worker2 //: BackgroundService
    {
        private readonly IAuditClient _client;

        public Worker2(IAuditClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _client.GetAsync(stoppingToken);

                await Task.Delay(20000, stoppingToken);
            }
        }
    }
}