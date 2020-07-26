using System.Threading;
using System.Threading.Tasks;

namespace Stef.AuditClient
{
    public interface IAuditClient
    {
        Task<string> GetAsync(CancellationToken cancellationToken);
    }
}