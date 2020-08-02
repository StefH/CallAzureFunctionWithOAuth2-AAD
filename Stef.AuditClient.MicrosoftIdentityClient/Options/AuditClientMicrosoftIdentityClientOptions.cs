namespace Stef.AuditClient.MicrosoftIdentityClient.Options
{
    public class AuditClientMicrosoftIdentityClientOptions
    {
        public string TenantId { get; set; } // = $"https://login.microsoftonline.com/{tenant}/oauth2/token",
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Resource { get; set; }
        public string BaseAddress { get; set; }
    }
}