using GlobalRoutes.SharedKernel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalRoutes.Core.Entities.Countries
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }
        public string OfficialName { get; set; }
        public string CountryCode { get; set; }
        public string IsoCode { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
