using Microsoft.EntityFrameworkCore;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Data.Repositories;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Runner.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RebateDbContext>(options =>
    options.UseInMemoryDatabase("Rebates"));

// Using depedency injection to adhere to SOLID principles
builder.Services.AddScoped<IRebateRepository, RebateRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRebateService, RebateService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger UI only in the development enviroment
if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    await DevelopmentDataSeeder.SeedAsync(
        scope.ServiceProvider.GetRequiredService<RebateDbContext>(), app.Logger,
        app.Lifetime.ApplicationStopping);

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

await app.RunAsync();
