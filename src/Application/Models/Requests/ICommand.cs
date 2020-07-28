using System;
using MediatR;

namespace Application.Models.Requests
{
    public interface ICommand<out TResult> : IRequest<TResult>
    {
        Guid CommandId { get; }
    }
}