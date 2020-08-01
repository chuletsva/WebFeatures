using System;

namespace Application.Interfaces.BackgroundJobs
{
    public interface IBackgroundJobManager
    {
        void Run<TJobArgument>(TJobArgument argument);
        void RunAfterDelay<TJobArgument>(TJobArgument argument, TimeSpan delay);
        void RunRecurrently<TJobArgument>(TJobArgument argument, string cronExpression);
    }
}
