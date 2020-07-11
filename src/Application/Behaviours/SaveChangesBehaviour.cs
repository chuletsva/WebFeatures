using Application.Infrastructure.Requests;
using Application.Interfaces.DataAccess;
using Application.Interfaces.Logging;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviours
{
    internal class SaveChangesBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IDbContext _db;

        public SaveChangesBehaviour(ILogger<TRequest> logger, IDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response = await next();

            try
            {
                await _db.SaveChangesAsync(cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while saving changes", ex);

                throw;
            }
        }
    }
}