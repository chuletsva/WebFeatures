namespace Application.Common.Interfaces.BackgroundJobs
{
    public interface IBackgroundJob
    {
    }

    public interface IBackgroundJob<in TJobArgument> : IBackgroundJob
        where TJobArgument : notnull
    {
        void Execute(TJobArgument argument);
    }
}