using InventorySales.Models;

namespace InventorySales.Service
{
    public interface iUser
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUserByName(string name);
        Task<IEnumerable<User>> GetAllUsersByRole(string role);
        Task Add(User user);
        Task Update(User user);
        Task Delete(User user);
        Task<User?> Authenticate(string name, string password);
    }
}
