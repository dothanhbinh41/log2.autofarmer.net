using Autolike.Options;
using LogJson.AutoFarmer.Repositories;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Win32;
using Newtonsoft.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
Register(builder.Services, builder.Configuration);
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


void Register(IServiceCollection services, IConfiguration configuration)
{
    services.AddTransient<IObjectSerializer, DefaultObjectSerializer>();
    services.AddTransient<IAutoFarmerDistributeCache, AutoFarmerDistributeCache>();
    services.Configure<MongoOptions>(mongoOptions =>
    {
        mongoOptions.ConnectionString = configuration["MongoDbConnection:ConnectionString"];
        mongoOptions.DatabaseName = configuration["MongoDbConnection:DatabaseName"];
    });
    services.AddSingleton<IAutolikeMongoClient, AutolikeMongoClient>();
    services.AddTransient(typeof(IMongoRepository<>), typeof(MongoBaseRepository<>));
    services.AddDistributedMemoryCache();
    services.AddMemoryCache();

    services.AddControllers()
        .AddJsonOptions(
            options => {
                options.JsonSerializerOptions.PropertyNamingPolicy =
                    SnakeCaseNamingPolicy.Instance;
            });
}

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    private readonly SnakeCaseNamingStrategy _newtonsoftSnakeCaseNamingStrategy
        = new SnakeCaseNamingStrategy();

    public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();

    public override string ConvertName(string name)
    {
        /* A conversion to snake case implementation goes here. */

        return _newtonsoftSnakeCaseNamingStrategy.GetPropertyName(name, false);
    }
}