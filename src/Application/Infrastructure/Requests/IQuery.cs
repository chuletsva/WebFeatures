using MediatR;

namespace Application.Infrastructure.Requests
{
    public interface IQuery<TResult> : IRequest<TResult>
    {
    }
}
