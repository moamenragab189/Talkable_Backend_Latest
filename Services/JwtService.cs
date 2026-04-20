using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Talkable.Data.Entities;
using Talkable.Data.Models;

namespace Talkable.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Claim> AddUserClaims(string username, UserType type,int userId)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(type.ToString()))
            {
                throw new ArgumentException("Username and type cannot be null or empty.");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, type.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }; 
            return claims;
        }
        public string CreateToken(List<Claim> claims)

        {
            if (claims == null || claims.Count == 0)
            {
                throw new ArgumentException("Claims cannot be null or empty.");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
                );
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
