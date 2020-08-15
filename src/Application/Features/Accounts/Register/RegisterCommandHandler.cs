using Domian.Entities.Accounts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.DataAccess;
using Application.Common.Interfaces.Logging;
using Application.Common.Interfaces.Security;
using Application.Common.Models.Dto;

namespace Application.Features.Accounts.Register
{
    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserInfoDto>
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

        public async Task<UserInfoDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _db.Users.AnyAsync(x => x.Email == request.Email, cancellationToken))
            {
                throw new ValidationException("Email already exists");
            }

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

            user.UserRoles.Add(new UserRole() { User = user, Role = role });

            _logger.LogInformation("{@User} registered", user);

            return new UserInfoDto()
            {
                Id = user.Id,
                Roles = new[] { role.Name }
            };
        }
    }
}