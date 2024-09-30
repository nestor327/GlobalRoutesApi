using GlobalRoutes.Core.Entities.Buses;
using GlobalRoutes.Core.Entities.Countries;
using GlobalRoutes.Core.Entities.Languages;
using GlobalRoutes.Core.Entities.Roles;
using GlobalRoutes.Core.Entities.Schedules;
using GlobalRoutes.Core.Entities.Stops;
using GlobalRoutes.Core.Entities.Subscriptions;
using GlobalRoutes.Core.Entities.Users;
using GlobalRoutes.Core.Entities.WeekDays;
using GlobalRoutes.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GlobalRoutes.Infrastructure.Contexts
{
    public class GlobalRoutesContext : IdentityDbContext<User, Role, string>
    {
        public GlobalRoutesContext(DbContextOptions options) : base(options)
        {
        }

        //Bus
        public virtual DbSet<Bus> Buses { get; set; }
        public virtual DbSet<BusType> BusTypes { get; set; }

        //Country 
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }

        //Language
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<UserLanguage> UserLanguages { get; set; }

        //Schedule
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<ScheduleWeekDay> ScheduleWeekDays { get; set; }

        //Stop
        public virtual DbSet<Stop> Stops { get; set; }

        //Subscription
        public virtual DbSet<Subscription> Subscriptions { get; set; }

        //TimeZone
        public virtual DbSet<Core.Entities.TimeZones.TimeZone> TimeZones { get; set; }

        //WeekDay
        public virtual DbSet<WeekDay> WeekDays { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetSimpleUnderscoreTableNameConvention(true);
            modelBuilder.ApplyUtcDateTimeConverter();

            modelBuilder.Entity<User>().Navigation(user => user.TimeZone).AutoInclude();

            base.OnModelCreating(modelBuilder);
        }
    }
}
