using System;

namespace Application.Interfaces.BackgroundJobs
{
    public abstract class BackgroundJobBase<TJobArgument> : IBackgroundJob<TJobArgument>
    {
        protected BackgroundJobBase()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; protected set; }

        public abstract void Execute(TJobArgument argument);
    }
}