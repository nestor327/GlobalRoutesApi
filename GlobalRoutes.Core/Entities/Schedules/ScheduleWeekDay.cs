using GlobalRoutes.Core.Entities.WeekDays;
using GlobalRoutes.SharedKernel.Entities;

namespace GlobalRoutes.Core.Entities.Schedules
{
    public class ScheduleWeekDay : BaseEntity
    {
        public bool IsActive { get; set; }
        public int? ScheduleId { get; set; }
        public int? WeekDayId { get; set; }
        
        public Schedule? Schedule { get; set; }
        public WeekDay? WeekDay { get; set; }
    }
}
