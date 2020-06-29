using Domian.Common;
using Domian.ValueObjects;

namespace Domian.Entities
{
    public class Manufacturer : Entity
    {
        public string OrganizationName { get; set; }
        public string HomePageUrl { get; set; }
        public Address StreetAddress { get; set; }
    }
}