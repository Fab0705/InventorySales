using InventorySales.Models;

namespace InventorySales.Service
{
    public interface iCategory
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task<Category> GetCategoryByName(string name);
        Task Add(Category category);
        Task Update(Category category);
        Task Delete(Category category);
    }
}
