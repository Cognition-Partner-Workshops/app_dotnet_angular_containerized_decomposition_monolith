using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderService.Infrastructure;
using OrderService.Services;

var builder = WebApplication.CreateBuilder(args);

/************* ADD SERVICES *************/

var connectionString = builder.Configuration.GetConnectionString("OrderDb") ??
    throw new InvalidOperationException("Connection string 'OrderDb' not found.");

builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Configure JWT Bearer authentication.
// The Authority should point to the monolith's OpenIddict OIDC discovery endpoint.
// In production, set Authentication:Authority to the monolith's public URL (e.g., https://monolith.example.com).
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["Authentication:Authority"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["Authentication:Audience"]
        };

        // In development, allow HTTP authority for local testing
        if (builder.Environment.IsDevelopment())
        {
            options.RequireHttpsMetadata = false;
        }
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Service API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAutoMapper(typeof(Program));

// Business Services
builder.Services.AddScoped<IOrdersService, OrdersService>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<OrderDbContext>();

var app = builder.Build();

/************* CONFIGURE REQUEST PIPELINE *************/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "Swagger UI - Order Service";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Service API V1");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");
app.MapHealthChecks("/ready");

app.Run();
