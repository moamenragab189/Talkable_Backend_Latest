using Talkable.Data.Models;
using Talkable.Data.Repositories;

namespace Talkable.Services
{
    public class AuthService
    {
        private readonly AuthRepository _authRepository;
        public AuthService(AuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task register(User user)
        {
            await _authRepository.register(user);
        }
        public async Task<User?> login(string email, string password)
        {
           var user= await _authRepository.login(email, password);
            if(user==null)
            {
                throw new Exception("Invalid email or password");
            }
            return user;
        }
    }
}
