﻿using System;
using System.Net.Http;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stef.AuditClient.AzureIdentity;

namespace ConsoleAppCallApi
{
    public class Worker3AzureIdentity : BackgroundService
    {
        private readonly IAuditClientAzureIdentity _client;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _f;

        public Worker3AzureIdentity(IAuditClientAzureIdentity client, IServiceProvider serviceProvider, IHttpClientFactory f)
        {
            _client = client;
            _serviceProvider = serviceProvider;
            _f = f;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var g = _f.CreateClient("google");
            var x = await g.GetAsync("", stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await _client.GetAsync(stoppingToken);

                var c2 = _serviceProvider.GetRequiredService<IAuditClientAzureIdentity>();
                await c2.GetAsync(stoppingToken);

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}