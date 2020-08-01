namespace Application.Common.Interfaces.BackgroundJobs
{
    public interface IBackgroundJob
    {
        string Id { get; }
    }

    public interface IBackgroundJob<in TJobArgument> : IBackgroundJob
    {
        void Execute(TJobArgument argument);
    }
}