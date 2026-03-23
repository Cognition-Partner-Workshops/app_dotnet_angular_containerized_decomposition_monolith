using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Infrastructure;
using ProductCatalogService.Models;

namespace ProductCatalogService.Services
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

        public async Task<Product?> UpdateProductAsync(int id, Product product)
        {
            var existing = await dbContext.Products.FindAsync(id);
            if (existing == null)
                return null;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Icon = product.Icon;
            existing.BuyingPrice = product.BuyingPrice;
            existing.SellingPrice = product.SellingPrice;
            existing.UnitsInStock = product.UnitsInStock;
            existing.IsActive = product.IsActive;
            existing.IsDiscontinued = product.IsDiscontinued;
            existing.ProductCategoryId = product.ProductCategoryId;
            existing.ParentId = product.ParentId;

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

        public async Task<ProductCategory?> UpdateCategoryAsync(int id, ProductCategory category)
        {
            var existing = await dbContext.ProductCategories.FindAsync(id);
            if (existing == null)
                return null;

            existing.Name = category.Name;
            existing.Description = category.Description;
            existing.Icon = category.Icon;

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
