using System;

namespace Application.Features.Products.GetProduct
{
    public class ProductInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid CurrencyId { get; set; }
        public Guid? PictureId { get; set; }
        public Guid ManufacturerId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid BrandId { get; set; }
    }
}