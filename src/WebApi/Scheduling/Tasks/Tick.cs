using Application.Common.Interfaces.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Scheduling.Tasks
{
    internal class Tick : IScheduledTask
    {
        private readonly ILogger<Tick> _logger;

        public Tick(ILogger<Tick> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tick");
        }
    }
}