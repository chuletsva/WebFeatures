using Application.Common.Interfaces.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Scheduling.Tasks
{
    internal class SampleScheduledTask : IScheduledTask
    {
        private readonly ILogger<SampleScheduledTask> _logger;

        public SampleScheduledTask(ILogger<SampleScheduledTask> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sample UI task finished");
        }
    }
}