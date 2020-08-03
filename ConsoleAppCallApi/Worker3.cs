using System;
using Microsoft.Extensions.Hosting;
using Stef.AuditClient.MicrosoftIdentityClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAppCallApi
{
    public class Worker3AzureIdentity : BackgroundService
    {
        private readonly IAuditClientMicrosoftIdentityClient _client;
        private readonly IServiceProvider _serviceProvider;

        public Worker3AzureIdentity(IAuditClientMicrosoftIdentityClient client, IServiceProvider serviceProvider)
        {
            _client = client;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _client.GetAsync(stoppingToken);

                var c2 = _serviceProvider.GetRequiredService<IAuditClientMicrosoftIdentityClient>();
                await c2.GetAsync(stoppingToken);

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}