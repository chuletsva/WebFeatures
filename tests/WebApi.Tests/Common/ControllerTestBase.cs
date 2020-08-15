using System;

namespace WebApi.Tests.Common
{
    public class ControllerTestBase : IDisposable
    {
        protected CustomWebApplicationFactory<Startup> Factory { get; }

        protected ControllerTestBase()
        {
            Factory = new CustomWebApplicationFactory<Startup>();
        }

        public void Dispose()
        {
            Factory.Dispose();
        }
    }
}
