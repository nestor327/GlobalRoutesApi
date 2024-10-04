using CsvHelper.Configuration;
using GlobalRoutes.Core.Entities.Buses;
using System.Globalization;

namespace GlobalRoutes.Infrastructure.Importer.Maps
{
    class BusMap : ClassMap<Bus>
    {
        public BusMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.UpdatedAt).Ignore();
            Map(m => m.UpdatedBy).Ignore();
            Map(m => m.CreatedAt).Ignore();
            Map(m => m.CreatedBy).Ignore();
        }
    }
}
