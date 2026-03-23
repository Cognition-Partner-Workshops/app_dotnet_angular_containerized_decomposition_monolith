using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Infrastructure;
using ProductCatalogService.Services;

var builder = WebApplication.CreateBuilder(args);

/************* ADD SERVICES *************/

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ProductCatalogDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddCors();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Business Services
builder.Services.AddScoped<IProductService, ProductService>();

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductCatalogDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        await dbContext.Database.MigrateAsync();
    }
    catch
    {
        logger.LogWarning("Could not run migrations. Ensuring database is created.");
        await dbContext.Database.EnsureCreatedAsync();
    }

    if (!await dbContext.ProductCategories.AnyAsync())
    {
        logger.LogInformation("Seeding product catalog data");

        var prodCat1 = new ProductCatalogService.Models.ProductCategory
        {
            Name = "None",
            Description = "Default category. Products that have not been assigned a category"
        };

        dbContext.ProductCategories.Add(prodCat1);
        await dbContext.SaveChangesAsync();

        var prod1 = new ProductCatalogService.Models.Product
        {
            Name = "BMW M6",
            Description = "Yet another masterpiece from the world's best car manufacturer",
            BuyingPrice = 109775,
            SellingPrice = 114234,
            UnitsInStock = 12,
            IsActive = true,
            ProductCategory = prodCat1
        };

        var prod2 = new ProductCatalogService.Models.Product
        {
            Name = "Nissan Patrol",
            Description = "A true man's choice",
            BuyingPrice = 78990,
            SellingPrice = 86990,
            UnitsInStock = 4,
            IsActive = true,
            ProductCategory = prodCat1
        };

        dbContext.Products.Add(prod1);
        dbContext.Products.Add(prod2);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Seeding product catalog data completed");
    }
}

/************* RUN APP *************/

app.Run();
