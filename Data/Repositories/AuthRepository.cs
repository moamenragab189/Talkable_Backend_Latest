using Microsoft.EntityFrameworkCore;
using Talkable.Data.Models;

namespace Talkable.Data.Repositories
{
    public class AuthRepository
    {
        private readonly MainContext _context;
        public AuthRepository(MainContext context)
        {
            _context = context;
        }
        public async Task register(User user)
        {
            _context.Tb_Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> login(string email, string password)
        {
            return await _context.Tb_Users.Where(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
        }
    }
}