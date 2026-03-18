// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using QuickApp.Core.Models.Shop;

namespace QuickApp.Core.Services.Shop
{
    public class OrdersService(HttpClient httpClient, ILogger<OrdersService> logger) : IOrdersService
    {
        public async Task<IEnumerable<Order>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Fetching all orders from order-service");
            var dtos = await httpClient.GetFromJsonAsync<IEnumerable<OrderServiceDto>>(
                "api/order", cancellationToken) ?? [];
            return dtos.Select(MapFromDto);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Fetching orders for customer {CustomerId} from order-service", customerId);
            var dtos = await httpClient.GetFromJsonAsync<IEnumerable<OrderServiceDto>>(
                $"api/order/customer/{customerId}", cancellationToken) ?? [];
            return dtos.Select(MapFromDto);
        }

        public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Fetching order {OrderId} from order-service", id);
            var response = await httpClient.GetAsync($"api/order/{id}", cancellationToken);
            if (!response.IsSuccessStatusCode)
                return null;
            var dto = await response.Content.ReadFromJsonAsync<OrderServiceDto>(cancellationToken);
            return dto is null ? null : MapFromDto(dto);
        }

        public async Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Creating order for customer {CustomerId} via order-service", order.CustomerId);
            var request = new CreateOrderServiceRequest(
                order.CustomerId,
                order.CashierId,
                order.Discount,
                order.Comments,
                order.OrderDetails.Select(d => new CreateOrderDetailServiceRequest(
                    d.ProductId,
                    d.Product?.Name,
                    d.UnitPrice,
                    d.Quantity,
                    d.Discount
                )).ToList()
            );

            var response = await httpClient.PostAsJsonAsync("api/order", request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var dto = await response.Content.ReadFromJsonAsync<OrderServiceDto>(cancellationToken)
                ?? throw new InvalidOperationException("Failed to deserialize created order");
            return MapFromDto(dto);
        }

        public async Task<Order> UpdateOrderAsync(int id, decimal discount, string? comments, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Updating order {OrderId} via order-service", id);
            var request = new UpdateOrderServiceRequest(discount, comments);
            var response = await httpClient.PutAsJsonAsync($"api/order/{id}", request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var dto = await response.Content.ReadFromJsonAsync<OrderServiceDto>(cancellationToken)
                ?? throw new InvalidOperationException("Failed to deserialize updated order");
            return MapFromDto(dto);
        }

        public async Task DeleteOrderAsync(int id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Deleting order {OrderId} via order-service", id);
            var response = await httpClient.DeleteAsync($"api/order/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        private static Order MapFromDto(OrderServiceDto dto)
        {
            var order = new Order
            {
                Id = dto.Id,
                CustomerId = dto.CustomerId,
                CashierId = dto.CashierId,
                Discount = dto.Discount,
                Comments = dto.Comments,
                CreatedDate = dto.CreatedDate,
                UpdatedDate = dto.UpdatedDate,
                Customer = new Customer { Name = string.Empty, Email = string.Empty }
            };

            foreach (var d in dto.OrderDetails)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    Id = d.Id,
                    ProductId = d.ProductId,
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Discount = d.Discount,
                    OrderId = dto.Id,
                    Order = order,
                    Product = new Product
                    {
                        Name = d.ProductName ?? string.Empty,
                        ProductCategory = new ProductCategory { Name = "Unknown" }
                    }
                });
            }

            return order;
        }
    }

    // DTOs matching the order-service contract
    internal record OrderServiceDto(
        int Id,
        int CustomerId,
        string? CashierId,
        decimal Discount,
        string? Comments,
        DateTime CreatedDate,
        DateTime UpdatedDate,
        IReadOnlyList<OrderDetailServiceDto> OrderDetails
    );

    internal record OrderDetailServiceDto(
        int Id,
        int ProductId,
        string? ProductName,
        decimal UnitPrice,
        int Quantity,
        decimal Discount
    );

    internal record CreateOrderServiceRequest(
        int CustomerId,
        string? CashierId,
        decimal Discount,
        string? Comments,
        IReadOnlyList<CreateOrderDetailServiceRequest> OrderDetails
    );

    internal record CreateOrderDetailServiceRequest(
        int ProductId,
        string? ProductName,
        decimal UnitPrice,
        int Quantity,
        decimal Discount
    );

    internal record UpdateOrderServiceRequest(
        decimal Discount,
        string? Comments
    );
}
