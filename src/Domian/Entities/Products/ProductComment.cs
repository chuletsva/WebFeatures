using Domian.Common;
using System;

namespace Domian.Entities.Products
{
    public class ProductComment : AuditableEntity
    {
        public string Body { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid? ParentCommentId { get; set; }
        public ProductComment ParentComment { get; set; }
    }
}
