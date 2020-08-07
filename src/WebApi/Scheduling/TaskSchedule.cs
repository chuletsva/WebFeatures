using NCrontab;
using System;

namespace WebApi.Scheduling
{
    internal class TaskSchedule
    {
        private readonly CrontabSchedule _schedule;

        public TaskSchedule(Type taskType, string cronExpression)
        {
            _schedule = CrontabSchedule.Parse(cronExpression);
            TaskType = taskType;
        }

        public Type TaskType { get; }
        public DateTime RunTime { get; private set; }

        public void ScheduleNextRunTime(DateTime baseTime)
        {
            RunTime = _schedule.GetNextOccurrence(baseTime);
        }
    }
}