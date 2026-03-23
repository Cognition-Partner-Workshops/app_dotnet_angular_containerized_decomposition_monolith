// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

namespace QuickApp.Core.Services.Shop
{
    public class ProductService(ProductCatalogApiClient apiClient) : IProductService
    {
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            return await apiClient.GetAllProductsAsync();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            return await apiClient.GetProductByIdAsync(id);
        }
    }
}
