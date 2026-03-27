using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer(); // لازم برای Minimal API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My Docker Web API",
        Version = "v1",
        Description = "Sample Minimal API with Swagger in Docker"
    });
});

var app = builder.Build();

// Configure Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Docker Web API V1");
    });
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");


var mongoConnection = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
var client = new MongoClient(mongoConnection);
var database = client.GetDatabase("testdb");
var collection = database.GetCollection<BsonDocument>("items");

app.MapPost("/add", async () =>
{
    var doc = new BsonDocument
    {
        { "name", "test item" },
        { "createdAt", DateTime.UtcNow }
    };

    await collection.InsertOneAsync(doc);
    return Results.Ok("Inserted");
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
