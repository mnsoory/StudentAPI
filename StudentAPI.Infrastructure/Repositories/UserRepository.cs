

using Microsoft.EntityFrameworkCore;
using StudentAPI.Domain.Entities;
using StudentAPI.Domain.Interfaces;
using StudentAPI.Infrastructure.Persistence;

namespace StudentAPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StudentAPIDbContext _context;
        public UserRepository(StudentAPIDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
