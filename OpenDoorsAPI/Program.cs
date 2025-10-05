using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using OpenDoorsAPI.Services;
using System.Text;

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
builder.Services.AddSingleton<RecruitmentService>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OpenDoors API", Version = "v1" });

    // ✅ Cho phép nhập JWT vào Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập JWT token vào đây (không cần 'Bearer ' ở đầu)",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
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

// ---------------------- JWT CONFIG ----------------------
var jwtSecret = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrEmpty(jwtSecret))
    throw new ArgumentNullException("JWT secret is missing!");

// ✅ Add JWT Authentication chính thức
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
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

// ✅ Bắt buộc: Authentication phải nằm trước Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
