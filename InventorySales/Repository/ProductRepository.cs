using InventorySales.Models;
using InventorySales.Service;
using Microsoft.EntityFrameworkCore;

namespace InventorySales.Repository
{
    public class ProductRepository : iProduct
    {
        private readonly DBContext dbContext = new DBContext();
        public async Task Add(Product product)
        {
            try
            {
                dbContext.Products.Add(product);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task Delete(Product product)
        {
            dbContext.Remove(product);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProducts() => await dbContext.Products.ToListAsync();

        public async Task<Product> GetProductById(int productId) => await dbContext.Products.FindAsync(productId);

        public async Task<Product> GetProductByName(string productName) => await dbContext.Products.FirstOrDefaultAsync(p => p.Name == productName);

        public async Task Update(Product product)
        {
            dbContext.Products.Update(product);
            await dbContext.SaveChangesAsync();
        }
    }
}
