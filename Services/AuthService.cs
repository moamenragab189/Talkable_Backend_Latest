using Talkable.Data.Entities;
using Talkable.Data.Repositories;

namespace Talkable.Services
{
    public class AuthService
    {
        private readonly AuthRepository _authRepository;
        private readonly EmailService _emailService;
        public AuthService(AuthRepository authRepository, EmailService emailService)
        {
            _authRepository = authRepository;
            _emailService = emailService;
        }
        public async Task register(User user)
        {
            await _authRepository.register(user);
        }
        public async Task<User?> login(string email, string password)
        {
            var user = await _authRepository.login(email, password);
            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }
            return user;
        }

        public async Task ForgotPassword(string email)
        {
            var user = await _authRepository.CheckEmail(email);
            if (user == null)
            {
                throw new Exception("Email not found");
            }
            string otp = Random.Shared.Next(100000, 999999).ToString();
            OTP userOTP = new OTP
            {
                UserId = user.User_Id,
                Code = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10)
            };
            await _authRepository.SaveOTP(userOTP);
            await _emailService.SendEmailAsync(email, "Password Reset OTP", otp);
        }
        public async Task<bool> VerifyOTP(string email, string otp)
        {
            var user = await _authRepository.CheckEmail(email);
            if (user == null)
            {
                throw new Exception("Email not found");
            }
            var userOTP = await _authRepository.GetOTPByUserId(user.User_Id);
            if (userOTP == null || userOTP.Code != otp)
            {
                return false;
            }
            await _authRepository.DeleteOTP(userOTP);
            return true;

        }

        internal async Task ResetPassword(string email, string newPassword)
        {
            var user = await _authRepository.CheckEmail(email);
            if (user == null)
            {
                throw new Exception("Email not found");
            }
            user.Password = newPassword;
            await _authRepository.UpdatePassword(user, newPassword);
        }
    }
}
