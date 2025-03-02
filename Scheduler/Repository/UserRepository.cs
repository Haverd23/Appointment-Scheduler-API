using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u =>u.Id == id);
            if (user == null) return false;
            _appDbContext.Users.Remove(user);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserByEmail(User user)
        {
            var email = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            return email;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(u =>u.Id == id);
        }

        public async Task<List<User>> GetUserAsync(string? role, string? email)
        {
            var query = _appDbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(role))
            {
                query = query.Where(u => u.Role == role);
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.Email == email);
            }

            return await query.ToListAsync();
        }



        public async Task<User> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(i => i.Id == id);
            if (existingUser == null)
            {
                return null;
            }
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            _appDbContext.Users.Update(existingUser);
             await _appDbContext.SaveChangesAsync();
            return existingUser;
        }

        public  async Task<List<User>> GetAllUsersAsync()
        {
            return await _appDbContext.Users.ToListAsync();

        }
    }
}
