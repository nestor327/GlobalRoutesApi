using CsvHelper.Configuration;
using GlobalRoutes.Core.Entities.Languages;
using System.Globalization;

namespace GlobalRoutes.Infrastructure.Importer.Maps
{
    public class LanguageMaps : ClassMap<Language>
    {
        public LanguageMaps()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.ArchivedAt).Ignore();
            Map(m => m.ArchivedBy).Ignore();
            Map(m => m.IsArchived).Ignore();
            Map(m => m.UpdatedAt).Ignore();
            Map(m => m.UpdatedBy).Ignore();
            Map(m => m.CreatedAt).Ignore();
            Map(m => m.CreatedBy).Ignore();
            Map(m => m.isDeleted).Ignore();
        }
    }
}
