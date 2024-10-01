using GlobalRoutes.Core.Entities.Countries;
using GlobalRoutes.Core.Entities.Languages;
using GlobalRoutes.Core.Entities.Subscriptions;
using Microsoft.AspNetCore.Identity;

namespace GlobalRoutes.Core.Entities.Users
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime Registered { get; set; }
        public int? CityId { get; set; }
        public int? TimeZoneId { get; set; }
        public string? UserImageUrl { get; set; }
        public int? SubscriptionId { get; set; }

        public virtual City? City { get; set; }
        public virtual Subscription? Subscription { get; set; }
        public virtual TimeZones.TimeZone? TimeZone { get; set; }
        public virtual ICollection<UserLanguage> UserLanguages { get; set; }

    }
}
