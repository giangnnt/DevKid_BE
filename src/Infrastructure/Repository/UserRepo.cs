using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevKid.src.Infrastructure.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly DevKidContext _context;
        public UserRepo(DevKidContext context)
        {
            _context = context;
        }
        public async Task<bool> AddUser(User user)
        {
            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            _context.Users.Remove(_context.Users.Find(id) ?? throw new Exception("User not found"));
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<User> GetUser(Guid id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("User not found");
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users
                .ToListAsync();
        }

        public async Task<bool> UpdateUser(User user)
        {
            _context.Users.Update(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
