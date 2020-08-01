using MediatR;

namespace Application.Common.Models.Requests
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
