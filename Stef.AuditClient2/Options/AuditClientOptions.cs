namespace AuditClient.Options
{
    public class AuditClientOptions
    {
        public string TenantId { get; set; }// = $"https://login.microsoftonline.com/{tenant}/oauth2/token",
        public string ClientId { get; set; }// clientId,
        public string ClientSecret { get; set; }//= clientSecret,
        public string Resource { get; set; }//
        public string BaseAddress { get; set; }
    }
}
