using Application.Common.Interfaces.BackgroundJobs;

namespace Application.Common.Models.BackgroundJobs
{
    internal class SampleBackgroundJob : IBackgroundJob<SampleBackgroundJobArgument>
    {
        public void Execute(SampleBackgroundJobArgument argument)
        {
            // do something
        }
    }

    internal class SampleBackgroundJobArgument
    {
    }
}