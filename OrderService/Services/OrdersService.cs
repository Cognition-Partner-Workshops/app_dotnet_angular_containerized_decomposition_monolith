using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure;
using OrderService.Models;

namespace OrderService.Services
{
    public class OrdersService(OrderDbContext dbContext) : IOrdersService
    {
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await dbContext.Orders
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await dbContext.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateOrderAsync(int id, Order order)
        {
            var existingOrder = await dbContext.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null)
                return null;

            existingOrder.Discount = order.Discount;
            existingOrder.Comments = order.Comments;
            existingOrder.CashierId = order.CashierId;
            existingOrder.CustomerId = order.CustomerId;

            // Remove order details that are no longer present
            var incomingIds = order.OrderDetails.Where(d => d.Id != 0).Select(d => d.Id).ToHashSet();
            var toRemove = existingOrder.OrderDetails.Where(d => !incomingIds.Contains(d.Id)).ToList();
            foreach (var detail in toRemove)
            {
                dbContext.OrderDetails.Remove(detail);
            }

            // Update existing and add new order details
            foreach (var detail in order.OrderDetails)
            {
                var existingDetail = existingOrder.OrderDetails.FirstOrDefault(d => d.Id == detail.Id && detail.Id != 0);
                if (existingDetail != null)
                {
                    existingDetail.UnitPrice = detail.UnitPrice;
                    existingDetail.Quantity = detail.Quantity;
                    existingDetail.Discount = detail.Discount;
                    existingDetail.ProductId = detail.ProductId;
                }
                else
                {
                    existingOrder.OrderDetails.Add(new OrderDetail
                    {
                        UnitPrice = detail.UnitPrice,
                        Quantity = detail.Quantity,
                        Discount = detail.Discount,
                        ProductId = detail.ProductId,
                        OrderId = id,
                        Order = existingOrder
                    });
                }
            }

            await dbContext.SaveChangesAsync();
            return existingOrder;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await dbContext.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return false;

            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
