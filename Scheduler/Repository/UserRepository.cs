using Scheduler.Data;
using Scheduler.Models;
using Scheduler.Repository.Interfaces;

namespace Scheduler.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly appDbContext _appDbContext;

        public UserRepository(appDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<User> CreateUser(User user)
        {
            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }
    }
}
