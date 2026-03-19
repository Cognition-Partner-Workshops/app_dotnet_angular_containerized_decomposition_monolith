using CustomerMicroservice.Infrastructure;
using CustomerMicroservice.Models;

namespace CustomerMicroservice.Services
{
    public class CustomerService(CustomerDbContext dbContext) : ICustomerService
    {
        public IEnumerable<Customer> GetAllCustomersData() => dbContext.Customers
            .OrderBy(c => c.Name)
            .ToList();
    }
}
