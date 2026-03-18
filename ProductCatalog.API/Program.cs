using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Data;
using ProductCatalog.API.Services;

var builder = WebApplication.CreateBuilder(args);

/************* ADD SERVICES *************/

var connectionString = builder.Configuration.GetConnectionString("ProductCatalogConnection") ??
    throw new InvalidOperationException("Connection string 'ProductCatalogConnection' not found.");

builder.Services.AddDbContext<ProductCatalogDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Business Services
builder.Services.AddScoped<IProductService, ProductService>();

// Database Seeder
builder.Services.AddTransient<DatabaseSeeder>();

// Add CORS
builder.Services.AddCors();

var app = builder.Build();

/************* CONFIGURE REQUEST PIPELINE *************/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.MapControllers();

/************* SEED DATABASE *************/

using var scope = app.Services.CreateScope();
try
{
    var dbSeeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await dbSeeder.SeedAsync();
}
catch (Exception ex)
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "An error occurred whilst creating/seeding the product catalog database");
    throw;
}

/************* RUN APP *************/

app.Run();
