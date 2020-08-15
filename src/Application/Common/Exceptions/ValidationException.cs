using System;
using System.Collections.Generic;
using Application.Common.Models.Results;
using FluentValidation.Results;

namespace Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationError Error { get; }

        public ValidationException(IEnumerable<ValidationFailure> failures)
        {
            Error = new ValidationError(failures);
        }

        public ValidationException(string message)
        {
            Error = new ValidationError(message);
        }
    }
}
