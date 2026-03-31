using Microsoft.EntityFrameworkCore;
using ProductService.Core.Infrastructure;
using ProductService.Core.Models;

namespace ProductService.Core.Services
{
    public class ProductService(ProductDbContext dbContext) : IProductService
    {
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await dbContext.Products
                .Include(p => p.ProductCategory)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await dbContext.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            dbContext.Products.Update(product);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product != null)
            {
                dbContext.Products.Remove(product);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync()
        {
            return await dbContext.ProductCategories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
    }
}
