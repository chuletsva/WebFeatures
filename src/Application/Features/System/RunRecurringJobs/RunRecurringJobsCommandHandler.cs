using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.BackgroundJobs;
using Application.Common.Models.BackgroundJobs;
using MediatR;

namespace Application.Features.System.RunRecurringJobs
{
    internal class RunRecurringJobsCommandHandler : IRequestHandler<RunRecurringJobsCommand, Unit>
    {
        private readonly IBackgroundJobManager _jobManager;

        public RunRecurringJobsCommandHandler(IBackgroundJobManager jobManager)
        {
            _jobManager = jobManager;
        }

        public Task<Unit> Handle(RunRecurringJobsCommand request, CancellationToken cancellationToken)
        {
            //_jobManager.RunRecurrently(
            //    "cc08b0ac-6888-4470-bdd9-d119c63d912a",
            //    new SampleBackgroundJobArgument(),
            //    CronExpressions.Daily);

            return Unit.Task;
        }
    }
}