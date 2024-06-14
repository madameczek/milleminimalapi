global using FluentValidation;
using System.Text.Json;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using milletest.App.Infrastructure.DataSource.DbContexts;
using milletest.App.Infrastructure.DataSource.Services;
using milletest.App.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

/*builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();*/

builder.Services.AddDbContext<DishesDbContext>(o => o.UseSqlite(
    builder.Configuration["ConnectionStrings:DishesDBConnectionString"]));
builder.Services.AddScoped<IDishRepository, DishRepository>();

builder.Services.AddFastEndpoints();

builder.Services
    .SwaggerDocument(options =>
    {
        options.MaxEndpointVersion = 1;
        options.DocumentSettings = settings =>
        {
            settings.DocumentName = "v1";
            settings.Title = "Test Millenium";
            settings.Version = "v1.0";
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseHttpsRedirection();

//app.UseAuthentication();

app.UseFastEndpoints(config =>
{
    config.Versioning.Prefix = "v";
    config.Versioning.DefaultVersion = 1;
    config.Versioning.PrependToRoute = true;
    config.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.Endpoints.RoutePrefix = "api";
});

app.UseSwaggerGen();

// Recreate and migrate DB on each run
// TODO remove on production
using (var scope = app.Services.GetService<IServiceScopeFactory>()!.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DishesDbContext>();
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}

app.Run();

public partial class Program { }