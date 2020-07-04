using Domian.Common;
using Domian.ValueObjects;

namespace Domian.Entities
{
    public class Shipper : Entity
    {
        public string OrganizationName { get; set; }
        public string HomePageUrl { get; set; }
        public string ContactPhone { get; set; }
        public Address HeadOffice { get; set; }
    }
}
