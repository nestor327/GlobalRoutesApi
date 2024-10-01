using GlobalRoutes.Core.Entities.Stops;
using GlobalRoutes.SharedKernel.Entities;

namespace GlobalRoutes.Core.Entities.Schedules
{
    public class Schedule : BaseEntity
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public double Duration { get; set; }
        public double OrigenLatitude { get; set; }
        public double OrigenLogitude { get; set; }
        public double DestinoLatitude { get; set; }
        public double DestinoLogitude { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<ScheduleWeekDay> ScheduleWeekDays { get; set; }
        public virtual ICollection<Stop> Stops { get; set; }
    }
}
