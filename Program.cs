using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Dotnet MongoDB Docker Sample",
        Version = "v1"
    });
});

// MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connString = builder.Configuration.GetConnectionString("MongoDb")
                     ?? "mongodb://localhost:27017";
    return new MongoClient(connString);
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dotnet MongoDB Docker Sample v1");
        c.RoutePrefix = string.Empty;   // Swagger UI at http://localhost:port/
    });
}

app.MapGet("/health", () => Results.Ok("Healthy! 🚀"))
   .WithName("HealthCheck");

app.Run();