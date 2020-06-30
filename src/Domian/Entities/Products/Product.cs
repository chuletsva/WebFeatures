using Domian.Common;
using Domian.Entities.Accounts;
using Domian.ValueObjects;
using System;

namespace Domian.Entities.Products
{
    public class Product : Entity, IAuditable
    {
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

        public DateTime CreateDate { get; set; }

        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public Guid UpdatedById { get; set; }
        public User UpdatedBy { get; set; }
    }
}
