﻿using CsvHelper;
using CsvHelper.Configuration;
using GlobalRoutes.Infrastructure.Importer.Maps;
using System.Globalization;
using System.Text;

namespace GlobalRoutes.Infrastructure.Importer
{
    public class CsvImporter<T>
    {
        public static List<T> FromCsvPath(string path)
        {
            using var reader = new StreamReader(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path),
                Encoding.GetEncoding("UTF-8"));

            return ReadProperties(reader);
        }

        public static List<T> FromString(string csvString)
        {
            using var reader = new StringReader(csvString);
            return ReadProperties(reader);
        }

        private static List<T> ReadProperties(TextReader reader)
        {
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                Encoding = Encoding.GetEncoding("UTF-8"),
                DetectDelimiter = true,
                BadDataFound = null,
                HeaderValidated = null,
                MissingFieldFound = null,
            };

            using var csv = new CsvReader(reader, csvConfiguration);
            csv.Context.RegisterClassMap<CountryMap>();
            csv.Context.RegisterClassMap<BusTypeMap>();
            csv.Context.RegisterClassMap<BusMap>();
            csv.Context.RegisterClassMap<LanguageMaps>();
            csv.Context.RegisterClassMap<ScheduleMap>();
            //csv.Context.RegisterClassMap<RoleTranslationMap>();
            //csv.Context.RegisterClassMap<JobRoleCategoryMap>();
            //csv.Context.RegisterClassMap<JobRoleMap>();
            //csv.Context.RegisterClassMap<BenefitMap>();
            //csv.Context.RegisterClassMap<SkillMap>();
            //csv.Context.RegisterClassMap<SeniorityMap>();
            //csv.Context.RegisterClassMap<AppClientMap>();
            //csv.Context.RegisterClassMap<IndustriesMap>();
            //csv.Context.RegisterClassMap<LanguageLevelMap>();
            //csv.Context.RegisterClassMap<StoryPointMap>();
            //csv.Context.RegisterClassMap<PriorityLevelMap>();
            //csv.Context.RegisterClassMap<GoalStatusCategoryMap>();
            //csv.Context.RegisterClassMap<ProjectStatusCategoryMap>();
            //csv.Context.RegisterClassMap<ProjectStatusMap>();

            return csv.GetRecords<T>().ToList();
        }
    }
}
