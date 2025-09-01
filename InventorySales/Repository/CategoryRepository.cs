using InventorySales.Models;
using InventorySales.Service;
using Microsoft.EntityFrameworkCore;

namespace InventorySales.Repository
{
    public class CategoryRepository : iCategory
    {
        private readonly DBContext dbContext = new DBContext();
        public async Task Add(Category category)
        {
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(Category category)
        {
            dbContext.Remove(category);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategories() => await dbContext.Categories.ToListAsync();

        public async Task<Category> GetCategoryById(int id) => await dbContext.Categories.FindAsync(id);

        public async Task<Category> GetCategoryByName(string name) => await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == name);

        public async Task Update(Category category)
        {
            dbContext.Update(category);
            await dbContext.SaveChangesAsync();
        }
    }
}
