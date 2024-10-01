using GlobalRoutes.Core.Entities.Languages;
using GlobalRoutes.Core.Entities.Schedules;
using GlobalRoutes.SharedKernel.Entities;

namespace GlobalRoutes.Core.Entities.Buses
{
    public class Bus : BaseEntity
    {
        public string Name { get; set; }
        public double Frecuency { get; set; }
        public bool IsActive { get; set; }
        public int? BusTypeId { get; set; }
        
        public virtual BusType? BusType { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
