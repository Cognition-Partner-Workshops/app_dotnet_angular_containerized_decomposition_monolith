// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using System.Net.Http.Json;
using QuickApp.Core.Models.Shop;

namespace QuickApp.Core.Services.Shop
{
    /// <summary>
    /// HTTP client proxy that forwards product requests to the Product Catalog microservice.
    /// </summary>
    public class ProductService(HttpClient httpClient) : IProductService
    {
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await httpClient.GetFromJsonAsync<IEnumerable<Product>>("api/products");
            return products ?? [];
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var response = await httpClient.GetAsync($"api/products/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync()
        {
            var categories = await httpClient.GetFromJsonAsync<IEnumerable<ProductCategory>>("api/productcategories");
            return categories ?? [];
        }

        public async Task<Product?> CreateProductAsync(Product product)
        {
            var response = await httpClient.PostAsJsonAsync("api/products", product);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            var response = await httpClient.PutAsJsonAsync($"api/products/{product.Id}", product);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await httpClient.DeleteAsync($"api/products/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
