using Application.Common.Interfaces.BackgroundJobs;

namespace Application.Common.Models.BackgroundJobs
{
    public class SampleBackgroundJob : IBackgroundJob<SampleBackgroundJobArgument>
    {
        public void Execute(SampleBackgroundJobArgument argument)
        {
            // do something
        }
    }

    public class SampleBackgroundJobArgument
    {
    }
}