using Application.Exceptions;
using Application.Infrastructure.Requests;
using Application.Interfaces.DataAccess;
using Application.Interfaces.Services;
using Domian.Entities.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviours
{
    internal class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IAuthorizedRequest
    {
        private readonly IDbContext _db;
        private readonly ICurrentUser _currentUser;

        public AuthorizationBehaviour(IDbContext db, ICurrentUser currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!_currentUser.IsAuthenticated)
            {
                throw new FailedAuthorizationException();
            }

            User user = await _db.Users.FindAsync(_currentUser.UserId, cancellationToken) ?? throw new FailedAuthorizationException();

            // TODO: check roles

            return await next();
        }
    }
}