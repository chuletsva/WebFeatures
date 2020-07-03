using System;

namespace Domian.Entities.Products
{
    public class ProductPicture
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid FileId { get; set; }
        public File File { get; set; }
    }
}
