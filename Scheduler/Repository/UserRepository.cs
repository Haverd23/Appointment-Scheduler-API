using Microsoft.EntityFrameworkCore;
using Scheduler.Data;
using Scheduler.Models;
using Scheduler.Repository.Interfaces;
using Scheduler.Services;

namespace Scheduler.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly appDbContext _appDbContext;
        private readonly PasswordHasher _passwordHasher;


        public UserRepository(appDbContext appDbContext, PasswordHasher passwordHasher)
        {
            _appDbContext = appDbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> CreateUser(User user)
        {
            var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null) throw new InvalidOperationException("Usuário com este email já existe.");
            user.Password = _passwordHasher.HashPassword(user.Password);
            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }
        public async Task<User> GetUserByEmail(User user)
        {
            var email = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            return email;
        }


    }
}
