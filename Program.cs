using Autolike.Options;
using LogJson.AutoFarmer.Repositories;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Win32;

var builder = WebApplication.CreateBuilder(args);
Register(builder.Services);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


void Register(IServiceCollection services)
{
    services.AddTransient<IObjectSerializer, DefaultObjectSerializer>();
    services.AddTransient<IAutoFarmerDistributeCache, AutoFarmerDistributeCache>();

    //services.Configure<MongoOptions>(mongoOptions);
    services.AddSingleton<IAutolikeMongoClient, AutolikeMongoClient>();
    services.AddTransient(typeof(IMongoRepository<>), typeof(MongoBaseRepository<>));
    services.AddDistributedMemoryCache();
    services.AddMemoryCache();
}