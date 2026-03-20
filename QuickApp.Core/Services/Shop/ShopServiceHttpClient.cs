using System.Net.Http.Json;

namespace QuickApp.Core.Services.Shop
{
    public class ShopServiceHttpClient(HttpClient httpClient) : IShopServiceClient
    {
        public async Task<bool> HasOrdersByCashierAsync(string cashierId)
        {
            var response = await httpClient.GetAsync($"api/order/by-cashier/{Uri.EscapeDataString(cashierId)}/exists");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
