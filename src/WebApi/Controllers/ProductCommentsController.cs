using Application.Features.ProductComments.CreateProductComment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application.Models.Results;
using WebApi.Controllers.Base;

namespace WebApi.Controllers
{
    /// <summary>
    /// Комментарии к товарам
    /// </summary>
    public class ProductCommentsController : BaseController
    {
        /// <summary>
        /// Создать комментарий
        /// </summary>
        /// <returns>Идентификатор созданного комментария</returns>
        /// <response code="201" cref="Guid">Успех</response>
        /// <response code="400" cref="ValidationError">Ошибка валидации</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([Required] CreateProductCommentCommand request)
        {
            return Created(await Mediator.Send(request));
        }
    }
}