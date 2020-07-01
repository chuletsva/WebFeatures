using Application.Interfaces.Logging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviours
{
    class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("{@Request} started", request);

            TResponse response = await next();

            _logger.LogInformation("{@Request} finished with response: {@Response}", request, response);

            return response;
        }
    }
}