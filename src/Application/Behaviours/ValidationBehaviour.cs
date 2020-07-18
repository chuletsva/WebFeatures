using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Infrastructure.Requests;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ValidationException = Application.Exceptions.ValidationException;

namespace Application.Behaviours
{
	internal class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : ICommand<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators;

		public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		public async Task<TResponse> Handle(
			TRequest request,
			CancellationToken cancellationToken,
			RequestHandlerDelegate<TResponse> next)
		{
			ValidationFailure[] errors = await GetErrors(request);

			if (errors.Length != 0) throw new ValidationException(errors);

			return await next();
		}

		private async Task<ValidationFailure[]> GetErrors(TRequest request)
		{
			ValidationFailure[] errors =
				(await Task.WhenAll(
					from validator in _validators
					select validator.ValidateAsync(request)))
			   .Where(x => !x.IsValid)
			   .SelectMany(x => x.Errors)
			   .ToArray();

			return errors;
		}
	}
}
