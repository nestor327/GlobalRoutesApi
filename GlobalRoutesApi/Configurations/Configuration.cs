using GlobalRoutes.Api.Configurations.swagger;
using GlobalRoutes.Core.Entities.Roles;
using GlobalRoutes.Core.Entities.Users;
using GlobalRoutes.Core.Interfaces.Account;
using GlobalRoutes.Core.Services.Account;
using GlobalRoutes.Infrastructure.Contexts;
using GlobalRoutes.Infrastructure.Importer;
using GlobalRoutes.Infrastructure.Repositories;
using GlobalRoutes.SharedKernel.Interfaces;
using GlobalRoutes.SharedKernel.Properties;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;


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
            services.AddTransient<IAccountService, AccountService>();

            //Importer
            services.AddTransient<Importer>();

            return services;
        }

        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // allows API return versions in the response header (api-supported-versions).
                config.ReportApiVersions = true;
            });

            //// Allows to discover versions
            services.AddVersionedApiExplorer(config =>
            {
                config.GroupNameFormat = "'v'VVV";
                config.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(config =>
            {
                config.OperationFilter<SwaggerDefaultValuesFilter>();
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }

        public static IServiceCollection AddLanguages(this IServiceCollection services)
        {
            const string enUSCulture = "en";
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo(enUSCulture),
                    new CultureInfo("es")
                };
                options.DefaultRequestCulture = new RequestCulture(culture: enUSCulture, uiCulture: enUSCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
                {
                    context.Request.Headers.TryGetValue(NameStrings.HeaderName_Language, out var value);
                    return await Task.FromResult(new ProviderCultureResult(value.ToString()));
                }));
            });
            return services;
        }
    }
}
