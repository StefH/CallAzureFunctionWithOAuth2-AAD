using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Queues;
using ConsoleAppStorageQueue.Options;
using Microsoft.Extensions.Options;

namespace ConsoleAppStorageQueue
{
    public interface IQueueSender
    {
        Task<string> SendAsync<T>(T value, CancellationToken cancellationToken = default);
    }

    class QueueSender : IQueueSender
    {
        private readonly QueueClient _client;

        public QueueSender(IOptions<QueueSenderOptions> options)
        {
            var queueSenderOptions = options.Value;

            var credentialQueueApi = new ClientSecretCredential(queueSenderOptions.TenantId, queueSenderOptions.ClientId, queueSenderOptions.ClientSecret);
            _client = new QueueClient(queueSenderOptions.QueueUri, credentialQueueApi);

        }

        public async Task<string> SendAsync<T>(T value, CancellationToken cancellationToken = default)
        {
            var result = await _client.SendMessageAsync("xxx", cancellationToken);

            return result.Value.MessageId;
        }
    }
}
