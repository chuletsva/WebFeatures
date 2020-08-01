using MediatR;
using System;
using Application.Common.Models.Requests;

namespace Application.Features.Products.UpdateProduct
{
    /// <summary>
    /// Редактировать товар
    /// </summary>
    public class UpdateProductCommand : CommandBase<Unit>, IRequireAuthorization
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal PriceAmount { get; set; }

        /// <summary>
        /// Валюта
        /// </summary>
        public Guid PriceCurrencyId { get; set; }

        /// <summary>
        /// Изображение товара
        /// </summary>
        public Guid? PictureId { get; set; }

        /// <summary>
        /// Идентификатор производителя
        /// </summary>
        public Guid ManufacturerId { get; set; }

        /// <summary>
        /// Идентификатор категории
        /// </summary>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Идентификатор бренда
        /// </summary>
        public Guid BrandId { get; set; }
    }
}