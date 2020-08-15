using System;
using MediatR;

namespace Application.Common.Models.Requests
{
    public abstract class CommandBase<TResult> : ICommand<TResult>, IRequireValidation
    {
        public Guid CommandId { get; }

        protected CommandBase()
        {
            CommandId = Guid.NewGuid();
        }
    }
}