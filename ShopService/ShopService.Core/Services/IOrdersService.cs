using ShopService.Core.Models;

namespace ShopService.Core.Services
{
    public interface IOrdersService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<bool> HasOrdersByCashierAsync(string cashierId);
    }
}
