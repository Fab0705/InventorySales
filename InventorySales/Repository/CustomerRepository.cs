using InventorySales.Models;
using InventorySales.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InventorySales.Repository
{
    public class CustomerRepository : iCustomer
    {
        private readonly DBContext dbContext = new DBContext();
        public async Task Add(Customer customer)
        {
            try
            {
                dbContext.Customers.Add(customer);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task Delete(Customer customer)
        {
            try
            {
                dbContext.Remove(customer);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers() => await dbContext.Customers.ToListAsync();

        public async Task<Customer> GetCustomerByEmail(string email) => await dbContext.Customers.FirstOrDefaultAsync(c => c.Email == email);

        public async Task<Customer> GetCustomerById(int id) => await dbContext.Customers.FindAsync(id);

        public async Task<Customer> GetCustomerByName(string name) => await dbContext.Customers.FirstOrDefaultAsync(c => (c.FirstName + " " + c.LastName) == name);

        public async Task<IEnumerable<Customer>> SearchCustomer(string name, int limit = 10)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
                return new List<Customer>();

            return await dbContext.Customers
            .Where(c =>
                c.FirstName.Contains(name) ||
                c.LastName.Contains(name) ||
                (c.FirstName + " " + c.LastName).Contains(name))
            .OrderBy(c => c.FirstName)
            .ThenBy(c => c.LastName)
            .Take(limit)
            .ToListAsync();
        }

        public async Task Update(Customer customer)
        {
            try
            {
                dbContext.Update(customer);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
