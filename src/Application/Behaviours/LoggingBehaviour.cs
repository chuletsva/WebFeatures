using Application.Interfaces.Logging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviours
{
    internal class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("{Request} started: {@Request}", typeof(TRequest).Name, request);

            TResponse response = await next();

            _logger.LogInformation("{Request} finished with response: {@Response}", typeof(TRequest).Name, response);

            return response;
        }
    }
}