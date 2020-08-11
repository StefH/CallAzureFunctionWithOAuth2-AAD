using System.Threading;
using System.Threading.Tasks;

namespace Stef.AuditClient.MicrosoftIdentityClient
{
    public interface IAccessTokenService
    {
        Task<string> GetTokenAsync(CancellationToken cancellationToken);
    }
}