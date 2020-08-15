using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Scheduling
{
    internal interface IScheduledTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}