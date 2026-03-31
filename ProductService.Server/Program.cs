using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using ProductService.Core.Infrastructure;
using ProductService.Core.Services;
using ProductService.Server.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

/************* ADD SERVICES *************/

var connectionString = builder.Configuration.GetConnectionString("ProductConnection") ??
                throw new InvalidOperationException("Connection string 'ProductConnection' not found.");

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

builder.Services.AddDbContext<ProductDbContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly));
});

// Configure OpenIddict validation to validate tokens from the monolith
builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.SetIssuer(builder.Configuration["Auth:Authority"]!);
        options.UseSystemNetHttp();
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    o.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

// Add CORS
builder.Services.AddCors();

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Service API", Version = "v1" });
});

builder.Services.AddAutoMapper(typeof(Program));

// Business Services
builder.Services.AddScoped<IProductService, ProductService.Core.Services.ProductService>();

// Other Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserIdAccessor, UserIdAccessor>();

// File Logger
builder.Logging.AddFile(builder.Configuration.GetSection("Logging"));

var app = builder.Build();

/************* CONFIGURE REQUEST PIPELINE *************/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "Swagger UI - Product Service";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Service API V1");
    });
}

app.UseHttpsRedirection();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

/************* SEED DATABASE *************/

using var scope = app.Services.CreateScope();
try
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    await dbContext.Database.MigrateAsync();
    await SeedProductDataAsync(dbContext);
}
catch (Exception ex)
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "An error occurred whilst creating/seeding Product database");
}

/************* RUN APP *************/

app.Run();

/************* SEED HELPER *************/

static async Task SeedProductDataAsync(ProductDbContext dbContext)
{
    if (!await dbContext.ProductCategories.AnyAsync())
    {
        var prodCat_1 = new ProductService.Core.Models.ProductCategory
        {
            Name = "None",
            Description = "Default category. Products that have not been assigned a category"
        };

        var prod_1 = new ProductService.Core.Models.Product
        {
            Name = "BMW M6",
            Description = "Yet another masterpiece from the world's best car manufacturer",
            BuyingPrice = 109775,
            SellingPrice = 114234,
            UnitsInStock = 12,
            IsActive = true,
            ProductCategory = prodCat_1
        };

        var prod_2 = new ProductService.Core.Models.Product
        {
            Name = "Nissan Patrol",
            Description = "A true man's choice",
            BuyingPrice = 78990,
            SellingPrice = 86990,
            UnitsInStock = 4,
            IsActive = true,
            ProductCategory = prodCat_1
        };

        dbContext.ProductCategories.Add(prodCat_1);
        dbContext.Products.Add(prod_1);
        dbContext.Products.Add(prod_2);

        await dbContext.SaveChangesAsync();
    }
}
