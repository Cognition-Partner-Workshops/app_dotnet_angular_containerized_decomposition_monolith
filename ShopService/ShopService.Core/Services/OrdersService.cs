using Microsoft.EntityFrameworkCore;
using ShopService.Core.Infrastructure;
using ShopService.Core.Models;

namespace ShopService.Core.Services
{
    public class OrdersService(ShopDbContext dbContext) : IOrdersService
    {
        public async Task<IEnumerable<Order>> GetAllOrdersAsync() => await dbContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderDetails).ThenInclude(d => d.Product)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();

        public async Task<Order?> GetOrderByIdAsync(int id) => await dbContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderDetails).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<bool> HasOrdersByCashierAsync(string cashierId) =>
            await dbContext.Orders.Where(o => o.CashierId == cashierId).AnyAsync();
    }
}
