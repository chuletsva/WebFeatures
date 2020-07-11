using Application.Infrastructure.Requests;

namespace Application.Tests.Common.Stubs.Requests
{
    public class CustomCommand<TResponse> : CommandBase<TResponse>
    {
    }
}
