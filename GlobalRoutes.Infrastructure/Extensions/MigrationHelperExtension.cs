using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace GlobalRoutes.Infrastructure.Extensions
{
    public static class MigrationHelperExtension
    {
        public static void Migrate(this DatabaseFacade database, bool isDev, ILogger logger)
        {
            if (isDev)
            {
                logger.LogInformation("In Development mode, performing automatic database migration.");
                database.Migrate();
            }
            else
            {
                logger.LogInformation(
                    "In Production mode, no migrations performed. If migrations are required, run them manually.");
                database.EnsureCreated();
            }
        }
    }
}
