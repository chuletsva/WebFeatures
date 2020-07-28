using System;
using Application.Models.Requests;

namespace Application.Features.Products.CreateProduct
{
    public class CreateProductCommand : CommandBase<Guid>, IRequireAuthorization
    {
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