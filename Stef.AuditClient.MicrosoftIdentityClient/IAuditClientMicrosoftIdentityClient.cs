using System.Threading;
using System.Threading.Tasks;

namespace Stef.AuditClient.MicrosoftIdentityClient
{
    public interface IAuditClientMicrosoftIdentityClient
    {
        Task<string> GetAsync(CancellationToken cancellationToken);
    }
}