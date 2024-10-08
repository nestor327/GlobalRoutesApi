using GlobalRoutes.Api.Middewares;
using GlobalRoutesApi.Configurations;
using GlobalRoutesApi.Configurations.Extensions;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDbContexts(builder.Configuration)
    .AddRepositories()
    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
    .AddServices()
    .AddCustomApiVersioning()
    .AddLanguages();

var app = builder.Build();

app.MigrateDatabase();
await app.SeedDatabaseAsync();

app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRequestLocalization();

app.UseMiddleware<LanguageMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
