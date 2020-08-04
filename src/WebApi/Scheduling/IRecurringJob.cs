using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Scheduling
{
    internal interface IRecurringJob
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}