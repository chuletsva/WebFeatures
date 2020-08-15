using Application.Features.Products.CreateProduct;
using Application.Features.Products.DeleteProduct;
using Application.Features.Products.GetProduct;
using Application.Features.Products.GetProductComments;
using Application.Features.Products.GetProductReviews;
using Application.Features.Products.GetProducts;
using Application.Features.Products.UpdateProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Models.Results;
using WebApi.Attributes;
using WebApi.Controllers.Base;

namespace WebApi.Controllers
{
    /// <summary>
    /// Товары
    /// </summary>
    public class ProductsController : BaseController
    {
        /// <summary>
        /// Получить товар
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Товар</returns>
        /// <response code="200" cref="ProductInfoDto">Успех</response>
        /// <response code="400" cref="ValidationError">Товар отсутствует</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProductInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetProductQuery() { Id = id }));
        }

        /// <summary>
        /// Получить товары
        /// </summary>
        /// <returns>Список товаров</returns>
        /// <response code="200" cref="IQueryable{ProductInfoDto}">Успех</response>
        [OData]
        [HttpGet]
        [ProducesResponseType(typeof(IQueryable<ProductListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetProductsQuery()));
        }

        /// <summary>
        /// Получить обзоры на товар
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Обзоры</returns>
        /// <response code="200" cref="IQueryable{ProductReviewInfoDto}">Успех</response>
        [OData]
        [HttpGet("{id:guid}/reviews")]
        [ProducesResponseType(typeof(IQueryable<ProductReviewInfoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviews(Guid id)
        {
            return Ok(await Mediator.Send(new GetProductReviewsQuery() { ProductId = id }));
        }

        /// <summary>
        /// Получить комментарии к товару
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Комментарии</returns>
        /// <response code="200" cref="IQueryable{ProductCommentInfoDto}">Успех</response>
        [OData]
        [HttpGet("{id:guid}/comments")]
        [ProducesResponseType(typeof(IQueryable<ProductCommentInfoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetComments(Guid id)
        {
            return Ok(await Mediator.Send(new GetProductCommentsQuery() { ProductId = id }));
        }

        /// <summary>
        /// Создать товар
        /// </summary>
        /// <returns>Идентификатор созданного товара</returns>
        /// <response code="201" cref="Guid">Успех</response>
        /// <response code="400" cref="ValidationError">Ошибка валидации</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([Required] CreateProductCommand request)
        {
            return Created(await Mediator.Send(request));
        }

        /// <summary>
        /// Редактировать товар
        /// </summary>
        /// <response code="200">Успех</response>
        /// <response code="400" cref="ValidationError">Ошибка валидации</response>
        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [Required] UpdateProductCommand request)
        {
            await Mediator.Send(request);

            return Ok();
        }

        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <response code="204">Успех</response>
        /// <response code="400" cref="ValidationError">Товар отсутствует</response>
        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteProductCommand() { Id = id });

            return NoContent();
        }
    }
}