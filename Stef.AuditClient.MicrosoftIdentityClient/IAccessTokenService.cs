using System.Threading;
using System.Threading.Tasks;

namespace Stef.AuditClient.MicrosoftIdentityClient
{
    internal interface IAccessTokenService
    {
        Task<string> GetAccessToken(CancellationToken cancellationToken);
    }
}