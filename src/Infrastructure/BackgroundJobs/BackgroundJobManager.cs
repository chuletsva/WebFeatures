using System;
using System.Collections.Generic;
using Application.Common.Interfaces.BackgroundJobs;
using Hangfire;
using Hangfire.States;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.BackgroundJobs
{
    internal class BackgroundJobManager : IBackgroundJobManager
    {
        private readonly IBackgroundJobClient _jobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IServiceProvider _services;

        public BackgroundJobManager(
            IBackgroundJobClient jobClient,
            IRecurringJobManager recurringJobManager,
            IServiceProvider services)
        {
            _jobClient = jobClient;
            _recurringJobManager = recurringJobManager;
            _services = services;
        }

        public void Run<TJobArgument>(TJobArgument argument)
        {
            foreach (var job in GetJobs<TJobArgument>())
            {
                _jobClient.Create(() => job.Execute(argument), new EnqueuedState());
            }
        }

        public void RunAfterDelay<TJobArgument>(TJobArgument argument, TimeSpan delay)
        {
            foreach (var job in GetJobs<TJobArgument>())
            {
                _jobClient.Create(() => job.Execute(argument), new ScheduledState(delay));
            }
        }

        public void RunRecurrently<TJobArgument>(TJobArgument argument, string cronExpression)
        {
            foreach (var job in GetJobs<TJobArgument>())
            {
                _recurringJobManager.AddOrUpdate(job.Id, () => job.Execute(argument), cronExpression);
            }
        }

        private IEnumerable<IBackgroundJob<TJobArgument>> GetJobs<TJobArgument>()
        {
            return _services.GetServices<IBackgroundJob<TJobArgument>>();
        }
    }
}