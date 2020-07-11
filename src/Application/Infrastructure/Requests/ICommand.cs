using MediatR;
using System;

namespace Application.Infrastructure.Requests
{
    public interface ICommand<out TResult> : IRequest<TResult>
    {
        Guid CommandId { get; }
    }

    public abstract class CommandBase<TResult> : ICommand<TResult>
    {
        public Guid CommandId { get; }

        protected CommandBase()
        {
            CommandId = Guid.NewGuid();
        }
    }
}