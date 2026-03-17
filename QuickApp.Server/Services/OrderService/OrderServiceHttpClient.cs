using System.Net.Http.Json;

namespace QuickApp.Server.Services.OrderService;

public class OrderServiceHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrderServiceHttpClient> _logger;

    public OrderServiceHttpClient(HttpClient httpClient, ILogger<OrderServiceHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<OrderServiceDto>> GetAllOrdersAsync()
    {
        try
        {
            var orders = await _httpClient.GetFromJsonAsync<IEnumerable<OrderServiceDto>>("api/order");
            return orders ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve orders from order-service");
            return [];
        }
    }

    public async Task<OrderServiceDto?> GetOrderByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<OrderServiceDto>($"api/order/{id}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve order {OrderId} from order-service", id);
            return null;
        }
    }

    public async Task<OrderServiceDto?> CreateOrderAsync(CreateOrderServiceDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/order", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderServiceDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create order via order-service");
            return null;
        }
    }

    public async Task<bool> UpdateOrderAsync(int id, UpdateOrderServiceDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/order/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update order {OrderId} via order-service", id);
            return false;
        }
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/order/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete order {OrderId} via order-service", id);
            return false;
        }
    }
}
