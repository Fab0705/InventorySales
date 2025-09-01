using InventorySales.Models;
using System.Collections.Generic;

namespace InventorySales.Service
{
    public interface iCustomer
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(int id);
        Task<Customer> GetCustomerByName(string name);
        Task<Customer> GetCustomerByEmail(string email);
        Task<IEnumerable<Customer>> SearchCustomer(string name, int limit = 10);
        Task Add(Customer customer);
        Task Update(Customer customer);
        Task Delete(Customer customer);
    }
}
