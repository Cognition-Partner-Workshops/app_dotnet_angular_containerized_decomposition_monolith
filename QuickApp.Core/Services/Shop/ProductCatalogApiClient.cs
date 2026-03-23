// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace QuickApp.Core.Services.Shop
{
    public class ProductCatalogApiClient(HttpClient httpClient, ILogger<ProductCatalogApiClient> logger)
    {
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("api/products");
                return products ?? [];
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching products from Product Catalog service");
                return [];
            }
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<ProductDto>($"api/products/{id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching product {ProductId} from Product Catalog service", id);
                return null;
            }
        }
    }
}
