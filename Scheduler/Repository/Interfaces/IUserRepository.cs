using Scheduler.Models;

namespace Scheduler.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
        Task<User> GetUserByEmail(User user);

        Task<List<User>> GetUserAsync(string role, string email);
        Task<User> GetUserById(int id);
        Task<User> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
        Task<List<User>> GetAllUsersAsync();


    }
}
