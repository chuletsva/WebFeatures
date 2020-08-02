using Application.Features.System.RunRecurringJobs;
using MediatR;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.HostedServices
{
    public class RecurringJobsService : BackgroundService
    {
        private readonly IMediator _mediator;

        public RecurringJobsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _mediator.Send(new RunRecurringJobsCommand(), stoppingToken);
        }
    }
}