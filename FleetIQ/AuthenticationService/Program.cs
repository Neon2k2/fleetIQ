using System.Text;
using AuthenticationService.API.Filters;
using AuthenticationService.API.Middleware;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Infrastructure.Messaging;
using AuthenticationService.Infrastructure.Persistence;
using AuthenticationService.Infrastructure.Persistence.Repositories;
using AuthenticationService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// **1. Add Controllers with Validation Filter**
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddScoped<ValidationFilter>();

// **2. Add Authentication & Authorization**
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// **3. Register Database Context**
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// **4. Register Repositories**
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

// **5. Register Services**
builder.Services.AddSingleton<EmailService>();  // Email Service
builder.Services.AddSingleton<RabbitMQConnection>(); // RabbitMQ Connection
builder.Services.AddScoped<IEventPublisher, RabbitMQEventPublisher>(); // Event Publisher

// **6. Register Middleware Services**
builder.Services.AddSingleton<ExceptionMiddleware>();
builder.Services.AddSingleton<JwtMiddleware>();

// **7. Register Swagger**
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authentication Service API", Version = "v1" });
});

// **8. Build Application**
var app = builder.Build();

// **9. Configure Middlewares in the Correct Order**
app.UseMiddleware<ExceptionMiddleware>(); // Global Exception Handling
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<JwtMiddleware>(); // Custom JWT Middleware (after built-in authentication)
app.UseAuthorization();

// **10. Configure Swagger UI**
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// **11. Map Controllers**
app.MapControllers();

// **12. Run Application**
app.Run();
