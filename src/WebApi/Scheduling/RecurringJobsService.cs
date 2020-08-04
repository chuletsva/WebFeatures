using Application.Common.Interfaces.CommonServices;
using Application.Features.System.RunRecurringJobs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Scheduling.Jobs;

namespace WebApi.Scheduling
{
    internal class RecurringJobsService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly List<RecurringJobSchedule> _jobSchedules;

        public RecurringJobsService(IServiceScopeFactory scopeFactory, IDateTime datetime)
        {
            _scopeFactory = scopeFactory;

            _jobSchedules = new List<RecurringJobSchedule>()
            {
                new RecurringJobSchedule(typeof(UIRecurringJob), "* * * * *")
            };

            _jobSchedules.ForEach(x => x.ScheduleNextRunTime(datetime.Now));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RunApplicationRecurringTasks(stoppingToken);
            await RunUIRecurringTasks(stoppingToken);
        }

        private async Task RunApplicationRecurringTasks(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();

            await mediator.Send(new RunRecurringJobsCommand(), stoppingToken);
        }

        private async Task RunUIRecurringTasks(CancellationToken stoppingToken)
        {
            var delay = TimeSpan.FromSeconds(1);

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var now = scope.ServiceProvider.GetService<IDateTime>().Now;

                    var jobsToRun = new List<IRecurringJob>();

                    foreach (RecurringJobSchedule schedule in _jobSchedules)
                    {
                        if (DateTimesAreEqual(schedule.NextRunTime, now))
                        {
                            schedule.ScheduleNextRunTime(baseTime: now);

                            jobsToRun.Add((IRecurringJob)scope.ServiceProvider.GetService(schedule.JobType));
                        }
                    }

                    await Task.WhenAll(jobsToRun.Select(x => x.ExecuteAsync(stoppingToken)));
                }

                await Task.Delay(delay);
            }
        }

        private bool DateTimesAreEqual(DateTime firstTime, DateTime secondTime)
        {
            return firstTime.Year == secondTime.Year
                && firstTime.Month == secondTime.Month
                && firstTime.Day == secondTime.Day
                && firstTime.Hour == secondTime.Hour
                && firstTime.Minute == secondTime.Minute;
        }
    }
}