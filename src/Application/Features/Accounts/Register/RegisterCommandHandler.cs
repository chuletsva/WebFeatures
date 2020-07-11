using Application.Constants;
using Application.Interfaces.DataAccess;
using Application.Interfaces.Logging;
using Application.Interfaces.Security;
using Domian.Entities.Accounts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Register
{
    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserCreateDto>
    {
        private readonly ILogger<RegisterCommand> _logger;
        private readonly IDbContext _db;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterCommandHandler(
            ILogger<RegisterCommand> logger,
            IDbContext db,
            IPasswordHasher passwordHasher)
        {
            _logger = logger;
            _db = db;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserCreateDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            string hash = _passwordHasher.ComputeHash(request.Password);

            var user = new User()
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = hash
            };

            await _db.Users.AddAsync(user, cancellationToken);

            Role role = await _db.Roles.SingleOrDefaultAsync(x => x.Name == AuthorizationConstants.Roles.Users, cancellationToken) ?? 
                        throw new InvalidOperationException("Cannot find role for new user");

            user.Roles.Add(new UserRole() { User = user, Role = role });

            _logger.LogInformation("{@User} registered", user);

            return new UserCreateDto()
            {
                Id = user.Id,
                Roles = new[] { role.Name }
            };
        }
    }
}