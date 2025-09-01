using InventorySales.Models;
using InventorySales.Service;
using InventorySales.Utils;
using Microsoft.EntityFrameworkCore;

namespace InventorySales.Repository
{
    public class UserRepository : iUser
    {
        private readonly DBContext dbContext = new DBContext();
        public async Task Add(User user)
        {
            try
            {
                user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task<User?> Authenticate(string name, string password)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == name);

            if (user == null) return null;

            bool isValid = PasswordHasher.VerifyPassword(user.PasswordHash, password);

            return isValid ? user : null;
        }

        public async Task Delete(User user)
        {
            try
            {
                dbContext.Remove(user);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers() => await dbContext.Users.ToListAsync();

        public async Task<IEnumerable<User>> GetAllUsersByRole(string role) => await dbContext.Users.Where(u => u.Role == role).ToListAsync();

        public async Task<User> GetUserById(int id) => await dbContext.Users.FindAsync(id);

        public async Task<User> GetUserByName(string name) => await dbContext.Users.FirstOrDefaultAsync(u => u.Username == name);

        public async Task Update(User user)
        {
            try
            {
                dbContext.Update(user);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
