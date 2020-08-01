using MediatR;
using System;

namespace Application.Models.Requests
{
    public abstract class CommandBase<TResult> : ICommand<TResult>, IRequireValidation
    {
        public Guid CommandId { get; }

        protected CommandBase()
        {
            CommandId = Guid.NewGuid();
        }
    }

    public abstract class CommandBase : CommandBase<Unit>
    {
    }
}