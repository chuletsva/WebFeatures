using Application.Interfaces.BackgroundJobs;

namespace Application.Models.BackgroundJobs
{
    internal class SampleBackgroundJob : BackgroundJobBase<SampleBackgroundJobArgument>
    {
        public SampleBackgroundJob()
        {
            Id = "97073359-6b34-46f5-871f-ab7d39495ead";
        }

        public override void Execute(SampleBackgroundJobArgument argument)
        {
            // do something
        }
    }

    internal class SampleBackgroundJobArgument
    {
    }
}
