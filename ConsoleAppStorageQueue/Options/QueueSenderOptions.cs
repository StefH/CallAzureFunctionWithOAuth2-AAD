using System;

namespace ConsoleAppStorageQueue.Options
{
    public class QueueSenderOptions
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public Uri QueueUri { get; set; }
    }
}