using Application.Features.Products.CreateProduct;
using Application.Features.Products.DeleteProduct;
using Application.Features.Products.GetProduct;
using Application.Features.Products.GetProducts;
using Application.Features.Products.UpdateProduct;
using Application.Infrastructure.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

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
        /// <response code="200" cref="IEnumerable{ProductInfoDto}">Успех</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetProductsQuery()));
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