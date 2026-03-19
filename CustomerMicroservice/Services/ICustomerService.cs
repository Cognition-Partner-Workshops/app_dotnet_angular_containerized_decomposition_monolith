using CustomerMicroservice.Models;

namespace CustomerMicroservice.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAllCustomersData();
    }
}
