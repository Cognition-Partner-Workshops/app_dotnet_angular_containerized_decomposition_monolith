using ProductCatalog.API.Models;

namespace ProductCatalog.API.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product? GetProductById(int id);
        IEnumerable<ProductCategory> GetAllCategories();
        Product CreateProduct(Product product);
        Product UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}
