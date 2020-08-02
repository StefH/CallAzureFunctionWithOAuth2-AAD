using System.Threading;
using System.Threading.Tasks;
using Stef.AuditClient;
using Stef.AuditClient.MicrosoftIdentityClient;

namespace ConsoleAppCallApi.NET461
{
    public class Worker2 //: BackgroundService
    {
        private readonly IAuditClientMicrosoftIdentityClient _client;

        public Worker2(IAuditClientMicrosoftIdentityClient client)
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