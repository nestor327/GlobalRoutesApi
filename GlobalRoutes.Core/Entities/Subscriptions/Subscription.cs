using GlobalRoutes.SharedKernel.Entities;

namespace GlobalRoutes.Core.Entities.Subscriptions
{
    public class Subscription : BaseEntity
    {
        public string Name { get; set; }
        public string Deccription { get; set; }
        public bool IsActive { get; set; }
    }
}
