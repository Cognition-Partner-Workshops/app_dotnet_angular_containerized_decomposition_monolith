using Microsoft.EntityFrameworkCore;
using ProductCatalog.Core.Infrastructure;
using ProductCatalog.Core.Models;

namespace ProductCatalog.Core.Services
{
    public class ProductCategoryService(ProductCatalogDbContext dbContext) : IProductCategoryService
    {
        public async Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync()
        {
            return await dbContext.ProductCategories
                .Include(c => c.Products)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<ProductCategory?> GetCategoryByIdAsync(int id)
        {
            return await dbContext.ProductCategories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ProductCategory> CreateCategoryAsync(ProductCategory category)
        {
            dbContext.ProductCategories.Add(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<ProductCategory?> UpdateCategoryAsync(ProductCategory category)
        {
            var existing = await dbContext.ProductCategories.FindAsync(category.Id);
            if (existing == null)
                return null;

            dbContext.Entry(existing).CurrentValues.SetValues(category);
            await dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await dbContext.ProductCategories.FindAsync(id);
            if (category == null)
                return false;

            dbContext.ProductCategories.Remove(category);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
