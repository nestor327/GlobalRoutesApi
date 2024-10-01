using GlobalRoutes.Infrastructure.Contexts;
using GlobalRoutes.Infrastructure.Importer;

namespace GlobalRoutesApi.Configurations.Extensions
{
    public static class SeedDataBaseExtension
    {
        /// <summary>
        ///     Seed the database with initial data.
        /// </summary>
        /// <param name="host">The <see cref="IHost" /> containing the <see cref="AppDbContext" /> to seed.</param>
        public static async Task SeedDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = services.GetRequiredService<GlobalRoutesContext>();
                var importer = services.GetRequiredService<Importer>();
                var env = services.GetRequiredService<IHostEnvironment>();

                // Add seed data to the database if not present.
                await importer.RunAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }
    }
}
