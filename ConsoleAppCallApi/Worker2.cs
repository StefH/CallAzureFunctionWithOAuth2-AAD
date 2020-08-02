using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Stef.AuditClient;

namespace ConsoleAppCallApi
{
    public class Worker2 : BackgroundService
    {
        private readonly IAuditClient _client;

        public Worker2(IAuditClient client)
        {
            _client = client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _client.GetAsync(stoppingToken);

                await Task.Delay(20000, stoppingToken);
            }
        }
    }
}