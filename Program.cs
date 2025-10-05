using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RailBook.Application.Services;
using RailBook.Core.Application.Mapping;
using RailBook.Core.Application.Services;
using RailBook.Core.Domain.Repositories;
using RailBook.External.Persistence.Database;
using RailBook.External.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register AutoMapper with DI container 
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 4️⃣ Register Repositories (DI)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
builder.Services.AddScoped<ITrainServiceRepository, TrainServiceRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceDetailsRepository, InvoiceDetailsRepository>();

// 5️⃣ Register Services (DI)
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PassengerService>();
builder.Services.AddScoped<TrainServiceService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<InvoiceService>();
builder.Services.AddScoped<InvoiceDetailsService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

app.UseCors("AllowAngularApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ⚠️ CRITICAL: Add this line to map controllers
app.MapControllers();

app.Run();