using Microsoft.EntityFrameworkCore;
using Talkable.Data.Entities;

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
        public async Task<User?> CheckEmail(string email)
        {
            return await _context.Tb_Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        internal async Task SaveOTP(OTP oTP)
        {
            _context.Tb_OTP.Add(oTP);
            await _context.SaveChangesAsync();
        }

        internal async Task<OTP?> GetOTPByUserId(int user_Id)
        {
            return await _context.Tb_OTP.Where(o => o.UserId == user_Id).FirstOrDefaultAsync();
        }

        public async Task DeleteOTP(OTP userOTP)
        {
            _context.Tb_OTP.Remove(userOTP);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePassword(User user, string newPassword)
        {
            user.Password = newPassword;
            _context.Tb_Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}