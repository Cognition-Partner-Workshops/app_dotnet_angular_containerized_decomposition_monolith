using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.Services;

var builder = WebApplication.CreateBuilder(args);

/************* ADD SERVICES *************/

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ProductCatalogDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Add CORS
builder.Services.AddCors();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Catalog API", Version = "v1" });
});

// Business Services
builder.Services.AddScoped<IProductService, ProductService>();

// Seeder
builder.Services.AddTransient<ProductCatalogSeeder>();

var app = builder.Build();

/************* CONFIGURE REQUEST PIPELINE *************/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "Swagger UI - Product Catalog API";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalog API V1");
    });
}

app.UseHttpsRedirection();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

/************* SEED DATABASE *************/

using var scope = app.Services.CreateScope();
try
{
    var dbSeeder = scope.ServiceProvider.GetRequiredService<ProductCatalogSeeder>();
    await dbSeeder.SeedAsync();
}
catch (Exception ex)
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "An error occurred whilst creating/seeding the Product Catalog database");

    throw;
}

/************* RUN APP *************/

app.Run();
