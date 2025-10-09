using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RailBook.Configuration;
using RailBook.DataAccess.Database;
using RailBook.DataAccess.Implementations;
using RailBook.DataAccess.Interfaces;
using RailBook.Manager.Implementations;
using RailBook.Manager.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ===== 1. Add Database Context =====
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseLazyLoadingProxies());

// ===== 2. Configure JWT Settings =====
/*Reads JWT configuration (from appsettings.json):

            "JwtSettings": {
                "Secret": "supersecretkey123",
                "Issuer": "RailBookServer",
                "Audience": "RailBookClient",
                "ExpiryMinutes": 60
            }


            JwtSettings is a POCO (Plain Old CLR Object) class holding those values.
            These are later used for token generation and validation.
            🔹 Why it matters
            Every JWT has:
            Issuer (iss) → who issued it (your backend)
            Audience (aud) → who should accept it (frontend or another API)
            Secret → used to create and verify the token’s signature
            ExpiryMinutes → how long the token is valid
 */
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// ===== 3. Configure JWT Authentication =====
builder.Services.AddAuthentication(options =>
{
    // Set JWT as the default authentication scheme

    /*🔹 What this means conceptually

            You’re telling ASP.NET Core:
            “Use JWT Bearer tokens for authentication by default.”
            That means whenever the framework checks if a user is logged in (via [Authorize]), it will look for a Bearer token in the request header.
     */
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false; // Set to true in production with HTTPS
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        ClockSkew = TimeSpan.Zero // No tolerance for expired tokens
    };

    // Custom event handlers (optional, for debugging)
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token validated successfully");
            return Task.CompletedTask;
        }
    };
});

// Add this line to access HttpContext in services
builder.Services.AddHttpContextAccessor();

// ===== 4. Add Authorization =====
builder.Services.AddAuthorization();

// ===== 5. Register Services =====
// Repositories
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
builder.Services.AddScoped<ITrainServiceRepository, TrainServiceRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceDetailsRepository, InvoiceDetailsRepository>();

// Business Services
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<ITrainServiceService, TrainServiceService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IInvoiceDetailsService, InvoiceDetailsService>();

// Authentication Services
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ===== 6. Add Controllers =====
builder.Services.AddControllers();

// ===== 7. Add FluentValidation =====
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// ===== 8. Configure Swagger with JWT Support =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RailBook API",
        Version = "v1",
        Description = "Railway Booking System API with JWT Authentication"
    });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token. Example: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// ===== 9. Add CORS (Optional, for frontend) =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ===== Configure Middleware Pipeline =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANT: Order matters!
app.UseCors("AllowAll"); // If using CORS
app.UseAuthentication(); // ⬅️ Must come BEFORE UseAuthorization
app.UseAuthorization();  // ⬅️ Must come AFTER UseAuthentication

app.MapControllers();

app.Run();