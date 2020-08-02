using System;

namespace Application.Common.Interfaces.BackgroundJobs
{
    public interface IBackgroundJobManager
    {
        void Run<TJobArgument>(TJobArgument argument);
        void RunAfterDelay<TJobArgument>(TJobArgument argument, TimeSpan delay);
        void RunRecurrently<TJobArgument>(string id, TJobArgument argument, string cronExpression);
    }
}
