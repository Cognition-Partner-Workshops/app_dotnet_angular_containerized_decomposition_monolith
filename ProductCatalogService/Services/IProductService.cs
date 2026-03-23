using ProductCatalogService.Models;

namespace ProductCatalogService.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product?> UpdateProductAsync(int id, Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync();
        Task<ProductCategory?> GetCategoryByIdAsync(int id);
        Task<ProductCategory> CreateCategoryAsync(ProductCategory category);
        Task<ProductCategory?> UpdateCategoryAsync(int id, ProductCategory category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
