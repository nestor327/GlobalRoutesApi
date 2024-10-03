using CsvHelper.Configuration;
using GlobalRoutes.Core.Entities.Schedules;
using System.Globalization;

namespace GlobalRoutes.Infrastructure.Importer.Maps
{
    public class ScheduleWeekDayMap : ClassMap<ScheduleWeekDay>
    {
        public ScheduleWeekDayMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.UpdatedAt).Ignore();
            Map(m => m.UpdatedBy).Ignore();
            Map(m => m.CreatedAt).Ignore();
            Map(m => m.CreatedBy).Ignore();
        }
    }
}
