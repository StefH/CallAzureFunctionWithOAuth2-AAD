using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace Stef.AuditClient.AzureIdentity.RetryPolicies
{
    internal static class AuditClientPolicies
    {
        private const int RetryCount = 3;

        public static IAsyncPolicy<HttpResponseMessage> RetryPolicy(IServiceProvider serviceProvider)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(RetryCount, i => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) =>
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<AuditClientAzureIdentity>>();
                    logger.LogWarning($"Request failed with {result.Exception}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
                });
                //.WaitAndRetryAsync(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}