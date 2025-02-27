using Scheduler.Models;

namespace Scheduler.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
    }
}
