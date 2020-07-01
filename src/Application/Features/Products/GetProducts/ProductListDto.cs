using System;

namespace Application.Features.Products.GetProducts
{
    public class ProductListDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Guid CurrencyId { get; set; }
        public string CurrencyCode { get; set; }

        public Guid? MainPictureId { get; set; }

        public Guid ManufacturerId { get; set; }
        public string ManufacturerOrganizationName { get; set; }

        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
    }
}