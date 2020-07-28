using MediatR;

namespace Application.Models.Requests
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
