using NCrontab;
using System;

namespace WebApi.Scheduling
{
    internal class RecurringJobSchedule
    {
        private readonly CrontabSchedule _schedule;

        public RecurringJobSchedule(Type jobType, string cronExpression)
        {
            _schedule = CrontabSchedule.Parse(cronExpression);
            JobType = jobType;
        }

        public Type JobType { get; }
        public DateTime NextRunTime { get; private set; }

        public void ScheduleNextRunTime(DateTime baseTime)
        {
            NextRunTime = _schedule.GetNextOccurrence(baseTime);
        }
    }
}