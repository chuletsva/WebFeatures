using Application.Common.Interfaces.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Scheduling.Jobs
{
    internal class UIRecurringJob : IRecurringJob
    {
        private readonly ILogger<UIRecurringJob> _logger;

        public UIRecurringJob(ILogger<UIRecurringJob> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sample UI task finished");
        }
    }
}