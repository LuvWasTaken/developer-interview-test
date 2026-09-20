using Smartwyre.DeveloperTest.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRebateService, RebateService>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger UI only in the development enviroment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
