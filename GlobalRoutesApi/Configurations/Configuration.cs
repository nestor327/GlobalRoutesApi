using GlobalRoutes.Core.Entities.Roles;
using GlobalRoutes.Core.Entities.Users;
using GlobalRoutes.Infrastructure.Contexts;
using GlobalRoutes.Infrastructure.Importer;
using GlobalRoutes.Infrastructure.Repositories;
using GlobalRoutes.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace GlobalRoutesApi.Configurations
{
    public static class Configuration
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var postgresConnectionString = configuration.GetConnectionString("development");

            services.AddDbContextPool<GlobalRoutesContext>(options =>
                options.UseNpgsql(postgresConnectionString,
                        x => { x.MigrationsAssembly("GlobalRoutes.Infrastructure.Migrations"); }), poolSize: 20
            );

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<GlobalRoutesContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //Core Services

            //Importer
            services.AddTransient<Importer>();

            return services;
        }
    }
}
