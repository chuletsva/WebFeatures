using System;
using MediatR;

namespace Application.Common.Models.Requests
{
    public interface ICommand<out TResult> : IRequest<TResult>
    {
        Guid CommandId { get; }
    }
}