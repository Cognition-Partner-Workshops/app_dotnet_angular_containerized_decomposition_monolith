using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Services
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
            var existingProduct = await dbContext.Products.FindAsync(product.Id);
            if (existingProduct == null)
                return null;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Icon = product.Icon;
            existingProduct.BuyingPrice = product.BuyingPrice;
            existingProduct.SellingPrice = product.SellingPrice;
            existingProduct.UnitsInStock = product.UnitsInStock;
            existingProduct.IsActive = product.IsActive;
            existingProduct.IsDiscontinued = product.IsDiscontinued;
            existingProduct.ProductCategoryId = product.ProductCategoryId;
            existingProduct.ParentId = product.ParentId;

            await dbContext.SaveChangesAsync();
            return existingProduct;
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
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
    }
}
