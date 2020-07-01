using Domian.Common;
using Domian.ValueObjects;
using System;
using System.Collections.Generic;

namespace Domian.Entities.Products
{
    public class Product : AuditableEntity
    {
        public Product()
        {
            Reviews = new HashSet<ProductReview>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public Money Price { get; set; }

        public Guid? MainPictureId { get; set; }
        public File MainPicture { get; set; }

        public Guid ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public Guid? CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }

        public ICollection<ProductReview> Reviews { get; private set; }
    }
}