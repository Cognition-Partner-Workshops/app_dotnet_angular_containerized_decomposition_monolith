// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using QuickApp.Core.Models.Shop;

namespace QuickApp.Core.Services.Shop
{
    public interface IOrdersService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
        Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task<Order> UpdateOrderAsync(int id, decimal discount, string? comments, CancellationToken cancellationToken = default);
        Task DeleteOrderAsync(int id, CancellationToken cancellationToken = default);
    }
}
