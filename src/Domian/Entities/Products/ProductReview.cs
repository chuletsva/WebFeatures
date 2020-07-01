using Domian.Common;
using Domian.Enums;
using System;

namespace Domian.Entities.Products
{
    public class ProductReview : AuditableEntity
    {
        public string Title { get; set; }
        public string Comment { get; set; }
        public ProductRating Rating { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
