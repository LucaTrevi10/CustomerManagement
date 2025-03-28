using CustomerManagementApp.Models;

namespace CustomerManagementApp.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomers();
        Task<Customer> GetCustomer(int id);
        Task AddCustomer(Customer customer);
        Task UpdateCustomer(int id, Customer customer);
        Task DeleteCustomer(int id);
        Task AddTagToCustomer(int customerId, string tagName);
        Task RemoveTagFromCustomer(int customerId, string name);
        Task<List<string>> GetCustomerTags(int customerId);
    }
}
