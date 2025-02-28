using Scheduler.Models;

namespace Scheduler.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
        Task<User> GetUserByEmail(User user);
    }
}
