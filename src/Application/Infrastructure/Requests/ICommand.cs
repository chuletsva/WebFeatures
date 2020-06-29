using MediatR;
using System;

namespace Application.Infrastructure.Requests
{
    public interface ICommand<TResult> : IRequest<TResult>
    {
        public Guid Id { get; }
    }

    public abstract class CommandBase<TResult> : ICommand<TResult>
    {
        public Guid Id { get; }

        protected CommandBase()
        {
            Id = Guid.NewGuid();
        }
    }
}