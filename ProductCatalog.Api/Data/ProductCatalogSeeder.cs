using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Data
{
    public class ProductCatalogSeeder(ProductCatalogDbContext dbContext, ILogger<ProductCatalogSeeder> logger)
    {
        public async Task SeedAsync()
        {
            await dbContext.Database.MigrateAsync();
            await SeedProductDataAsync();
        }

        private async Task SeedProductDataAsync()
        {
            if (!await dbContext.ProductCategories.AnyAsync())
            {
                logger.LogInformation("Seeding product catalog data");

                var prodCat_1 = new ProductCategory
                {
                    Name = "None",
                    Description = "Default category. Products that have not been assigned a category"
                };

                var prod_1 = new Product
                {
                    Name = "BMW M6",
                    Description = "Yet another masterpiece from the world's best car manufacturer",
                    BuyingPrice = 109775,
                    SellingPrice = 114234,
                    UnitsInStock = 12,
                    IsActive = true,
                    ProductCategory = prodCat_1
                };

                var prod_2 = new Product
                {
                    Name = "Nissan Patrol",
                    Description = "A true man's choice",
                    BuyingPrice = 78990,
                    SellingPrice = 86990,
                    UnitsInStock = 4,
                    IsActive = true,
                    ProductCategory = prodCat_1
                };

                dbContext.Products.Add(prod_1);
                dbContext.Products.Add(prod_2);

                await dbContext.SaveChangesAsync();

                logger.LogInformation("Seeding product catalog data completed");
            }
        }
    }
}
