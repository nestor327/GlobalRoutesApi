using CsvHelper.Configuration;
using GlobalRoutes.Core.Entities.Schedules;
using System.Globalization;

namespace GlobalRoutes.Infrastructure.Importer.Maps
{
    public class ScheduleMap : ClassMap<Schedule>
    {
        public ScheduleMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.UpdatedAt).Ignore();
            Map(m => m.UpdatedBy).Ignore();
            Map(m => m.CreatedAt).Ignore();
            Map(m => m.CreatedBy).Ignore();
            Map(m => m.ArchivedAt).Ignore();
            Map(m => m.isDeleted).Ignore();
            Map(m => m.IsArchived).Ignore();
        }
    }
}
