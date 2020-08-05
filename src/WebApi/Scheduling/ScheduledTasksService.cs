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
using WebApi.Scheduling.Tasks;

namespace WebApi.Scheduling
{
    internal class ScheduledTasksService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly List<TaskSchedule> _schedules;

        public ScheduledTasksService(IServiceScopeFactory scopeFactory, IDateTime datetime)
        {
            _scopeFactory = scopeFactory;

            _schedules = new List<TaskSchedule>()
            {
                new TaskSchedule(typeof(SampleScheduledTask), "* * * * *")
            };

            _schedules.ForEach(x => x.ScheduleNextRunTime(datetime.Now));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RunApplicationScheduledTasks(stoppingToken);
            await RunScheduledTasks(stoppingToken);
        }

        private async Task RunApplicationScheduledTasks(CancellationToken stoppingToken)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();

            await mediator.Send(new RunRecurringJobsCommand(), stoppingToken);
        }

        private async Task RunScheduledTasks(CancellationToken stoppingToken)
        {
            var delay = TimeSpan.FromSeconds(1);

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    DateTime now = scope.ServiceProvider.GetService<IDateTime>().Now;

                    var tasksToRun = new List<IScheduledTask>();

                    foreach (TaskSchedule schedule in _schedules)
                    {
                        if (DateTimesAreEqual(schedule.RunTime, now))
                        {
                            schedule.ScheduleNextRunTime(baseTime: now);

                            tasksToRun.Add((IScheduledTask)scope.ServiceProvider.GetService(schedule.TaskType));
                        }
                    }

                    await Task.WhenAll(tasksToRun.Select(x => x.ExecuteAsync(stoppingToken)));
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