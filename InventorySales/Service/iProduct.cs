using InventorySales.Models;

namespace InventorySales.Service
{
    public interface iProduct
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int productId);
        Task<Product> GetProductByName(string productName);
        Task Add(Product product);
        Task Update(Product product);
        Task Delete(Product product);
    }
}
