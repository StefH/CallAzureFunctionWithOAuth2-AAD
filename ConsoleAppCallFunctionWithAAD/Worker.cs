﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleAppCallFunctionWithAAD
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public Worker(ILogger<Worker> logger, IHttpClientFactory factory)
        {
            _logger = logger;
            _clientFactory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var client = _clientFactory.CreateClient("client");
                var response = await client.PostAsync("HttpTrigger1", new StringContent("{ \"name\": \"x\" }"), stoppingToken);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("API response: {response}", content);
                }
                else
                {
                    _logger.LogError("API returned: {statusCode}", response.StatusCode);
                }

                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}