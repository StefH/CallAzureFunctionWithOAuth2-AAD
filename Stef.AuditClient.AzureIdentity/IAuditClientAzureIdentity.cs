using System.Threading;
using System.Threading.Tasks;

namespace Stef.AuditClient.AzureIdentity
{
    public interface IAuditClientAzureIdentity
    {
        Task<string> GetAsync(CancellationToken cancellationToken);
    }
}