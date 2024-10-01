

using GlobalRoutes.Infrastructure.Contexts;
using GlobalRoutes.Infrastructure.Extensions;

namespace GlobalRoutesApi.Configurations.Extensions
{
    public static class MigrateDatabaseExtensions
    {
        /// <summary>
        ///     Perform automatic migrations if in Development environment, otherwise simply
        ///     EnsureCreated as migrations should not be performed by the application in
        ///     production environments.
        /// </summary>
        /// <param name="host">The <see cref="IHost" /> containing the <see cref="AppDbContext" /> to migrate.</param>
        public static void MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var globalRoutesContext = services.GetRequiredService<GlobalRoutesContext>();
                var membersEnv = services.GetRequiredService<IWebHostEnvironment>();

                // Migrate the database if it's in development, otherwise EnsureCreated
                // TODO: Temporarily allow migration even in Release builds.
                globalRoutesContext.Database.Migrate(membersEnv.IsDevelopment() || membersEnv.IsProduction(), logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred migrating the DB.");
            }
        }
    }
}
