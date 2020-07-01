using Application.Infrastructure.Results;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationError Error { get; }

        public ValidationException(IEnumerable<ValidationFailure> failures)
        {
            Error = new ValidationError(failures);
        }

        public ValidationException(string message) : base(message)
        {
        }
    }
}
