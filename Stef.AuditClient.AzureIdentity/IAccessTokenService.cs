using System.Threading;
using System.Threading.Tasks;

namespace Stef.AuditClient.AzureIdentity
{
    public interface IAccessTokenService
    {
        Task<string> GetTokenAsync(CancellationToken cancellationToken);
    }
}