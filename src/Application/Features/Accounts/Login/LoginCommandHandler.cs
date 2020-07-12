using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces.DataAccess;
using Application.Interfaces.Logging;
using Application.Interfaces.Security;
using Domian.Entities.Accounts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Accounts.Login
{
	internal class LoginCommandHandler : IRequestHandler<LoginCommand, UserLoginDto>
	{
		private readonly IDbContext _db;
		private readonly ILogger<LoginCommand> _logger;
		private readonly IPasswordHasher _passwordHasher;

		public LoginCommandHandler(
			ILogger<LoginCommand> logger,
			IDbContext db,
			IPasswordHasher passwordHasher)
		{
			_logger = logger;
			_db = db;
			_passwordHasher = passwordHasher;
		}

		public async Task<UserLoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
		{
			User user = await _db.Users
			   .Include(x => x.Roles)
			   .ThenInclude(x => x.Role)
			   .SingleOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

			if (user == null || !_passwordHasher.Verify(user.PasswordHash, request.Password))
				throw new ValidationException("Wrong login or password");

			_logger.LogInformation("{@User} signed in", user);

			return new UserLoginDto
			{
				Id = user.Id,
				Roles = user.Roles.Select(x => x.Role.Name).ToArray()
			};
		}
	}
}
