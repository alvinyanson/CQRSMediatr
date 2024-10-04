using Microsoft.EntityFrameworkCore;
using CQRSMediatr.DataService.Data;
using CQRSMediatr.DataService.Repositories.Interfaces;
using CQRSMediatr.DataService.Repositories;
using CQRSMediatr.API.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// mapster
builder.Services.RegisterMapsterConfiguration();

// connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// initialize db context in DI Container
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

PrepDB.PrepPopulation(app, builder.Environment.IsProduction());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
