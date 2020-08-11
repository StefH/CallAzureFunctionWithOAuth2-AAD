using System;

namespace Stef.AuditClient.AzureIdentity.Options
{
    public class AuditClientMicrosoftIdentityClientOptions
    {
        public string TenantId { get; set; } 
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Resource { get; set; }
        public Uri BaseAddress { get; set; }
        public string HttpClientName { get; set; }
    }

    //public class AccessTokenServiceOptions
    //{
    //    public string TenantId { get; set; }
    //    public string ClientId { get; set; }
    //    public string ClientSecret { get; set; }
    //    public string Resource { get; set; }
    //}
}