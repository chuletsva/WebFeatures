using Application.Features.ProductReviews.CreateProductReview;
using Application.Infrastructure.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    /// <summary>
    /// Обзоры на товары
    /// </summary>
    public class ProductReviewsController : BaseController
    {
        /// <summary>
        /// Создать обзор
        /// </summary>
        /// <returns>Идентификатор созданного обзора</returns>
        /// <response code="201">Успех</response>
        /// <response code="400" cref="ValidationError">Ошибка валидации</response>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([Required] CreateProductReviewCommand request)
        {
            return Created(await Mediator.Send(request));
        }
    }
}