using GlobalRoutes.SharedKernel.Entities;

namespace GlobalRoutes.Core.Entities.Countries
{
    public class City : BaseEntity
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public int? CountryId { get; set; }

        public virtual Country? Country { get; set; }
    }
}
