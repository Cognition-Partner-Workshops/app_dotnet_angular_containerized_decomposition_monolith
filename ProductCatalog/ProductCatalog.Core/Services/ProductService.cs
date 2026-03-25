using Microsoft.EntityFrameworkCore;
using ProductCatalog.Core.Infrastructure;
using ProductCatalog.Core.Models;

namespace ProductCatalog.Core.Services
{
    public class ProductService(ProductCatalogDbContext dbContext) : IProductService
    {
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Parent)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Parent)
                .Include(p => p.Children)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            var existing = await dbContext.Products.FindAsync(product.Id);
            if (existing == null)
                return null;

            dbContext.Entry(existing).CurrentValues.SetValues(product);
            await dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null)
                return false;

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
