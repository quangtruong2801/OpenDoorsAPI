using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using OpenDoorsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------------------- CONFIGURATION ----------------------

// MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration["MongoDB:ConnectionString"];
    if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException("MongoDB connection string is null!");
    return new MongoClient(connectionString);
});

builder.Services.AddSingleton(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var databaseName = builder.Configuration["MongoDB:DatabaseName"];
    if (string.IsNullOrEmpty(databaseName))
        throw new ArgumentNullException("MongoDB database name is null!");
    return client.GetDatabase(databaseName);
});

// Cloudinary
builder.Services.AddSingleton<CloudinaryService>();

// Other Services
builder.Services.AddSingleton<MemberService>();
builder.Services.AddSingleton<TeamService>();
builder.Services.AddSingleton<JobService>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OpenDoors API",
        Version = "v1"
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Vite frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ---------------------- MIDDLEWARE ----------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenDoors API v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
