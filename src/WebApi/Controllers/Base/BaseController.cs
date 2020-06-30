using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        private IMediator _mediator;
    }
}