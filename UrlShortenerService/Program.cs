using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Service;
using UrlShortenerService.Data;
using UrlShortenerService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("UrlShortenerDb")));

builder.Services.AddScoped<IUrlMappingRepository, UrlMappingRepository>();
builder.Services.AddScoped<IUrlShortenerService, UrlShortener.API.Service.UrlShortenerService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

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
