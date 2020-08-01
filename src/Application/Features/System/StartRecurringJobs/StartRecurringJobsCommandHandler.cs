using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.BackgroundJobs;
using Application.Models.BackgroundJobs;
using MediatR;

namespace Application.Features.System.StartRecurringJobs
{
    internal class StartRecurringJobsCommandHandler : IRequestHandler<StartRecurringJobsCommand, Unit>
    {
        private readonly IBackgroundJobManager _jobManager;

        public StartRecurringJobsCommandHandler(IBackgroundJobManager jobManager)
        {
            _jobManager = jobManager;
        }

        public Task<Unit> Handle(StartRecurringJobsCommand request, CancellationToken cancellationToken)
        {
            _jobManager.RunRecurrently(new SampleBackgroundJobArgument(), CronExpressions.Daily);

            return Unit.Task;
        }
    }
}