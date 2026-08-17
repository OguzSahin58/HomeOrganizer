using HomeOrganizer.Api.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddValidation();

builder.AddSeedingDb();

var app = builder.Build();

app.MapControllers();

app.MigrateDb();

app.Run();
