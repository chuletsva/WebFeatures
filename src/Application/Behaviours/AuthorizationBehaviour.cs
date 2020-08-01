using Application.Exceptions;
using Application.Interfaces.DataAccess;
using Domian.Entities.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.CommonServices;
using Application.Models.Requests;

namespace Application.Behaviours
{
    internal class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequireAuthorization
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
            if (!_currentUser.IsAuthenticated) throw new FailedAuthorizationException();

            User user = await _db.Users.FindAsync(new object[] { _currentUser.UserId }, cancellationToken);

            if (user == null) throw new FailedAuthorizationException();

            // TODO: check roles

            return await next();
        }
    }
}