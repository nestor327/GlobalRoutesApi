using GlobalRoutes.SharedKernel.Entities;

namespace GlobalRoutes.Core.Entities.Buses
{
    public class BusType : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
