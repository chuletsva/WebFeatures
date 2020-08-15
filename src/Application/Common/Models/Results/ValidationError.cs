using System.Collections.Generic;
using FluentValidation.Results;

namespace Application.Common.Models.Results
{
    public class ValidationError
    {
        public string Message { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();

        private ValidationError() { } // for serialization

        public ValidationError(string message)
        {
            Message = message;
        }

        public ValidationError(IEnumerable<ValidationFailure> failures)
        {
            foreach (ValidationFailure failure in failures)
            {
                if (!Errors.ContainsKey(failure.PropertyName))
                    Errors[failure.PropertyName] = new List<string>();

                Errors[failure.PropertyName].Add(failure.ErrorMessage);
            }
        }
    }
}
