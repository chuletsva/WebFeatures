using MediatR;

namespace Application.Infrastructure.Requests
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
