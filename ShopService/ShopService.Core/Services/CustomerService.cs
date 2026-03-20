using Microsoft.EntityFrameworkCore;
using ShopService.Core.Infrastructure;
using ShopService.Core.Models;

namespace ShopService.Core.Services
{
    public class CustomerService(ShopDbContext dbContext) : ICustomerService
    {
        public IEnumerable<Customer> GetTopActiveCustomers(int count) => throw new NotImplementedException();

        public IEnumerable<Customer> GetAllCustomersData() => dbContext.Customers
                .Include(c => c.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(d => d.Product)
                .AsSingleQuery()
                .OrderBy(c => c.Name)
                .ToList();
    }
}
