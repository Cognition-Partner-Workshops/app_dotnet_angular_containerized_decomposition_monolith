using Microsoft.EntityFrameworkCore;
using ShopService.Core.Infrastructure;
using ShopService.Core.Models;

namespace ShopService.Core.Services
{
    public class ProductService(ShopDbContext dbContext) : IProductService
    {
        public async Task<IEnumerable<Product>> GetAllProductsAsync() => await dbContext.Products
            .Include(p => p.ProductCategory)
            .OrderBy(p => p.Name)
            .ToListAsync();

        public async Task<Product?> GetProductByIdAsync(int id) => await dbContext.Products
            .Include(p => p.ProductCategory)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
