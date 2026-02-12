using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talkable.Data.Models;
using Talkable.Services;

namespace Talkable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService) 
        {
            _authService = authService;
        }
        [HttpPost("register")]  
        public async Task<IActionResult> register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authService.register(user);
            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(string email,string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user= await _authService.login(email, password);
            return Ok(user);
        }
    }
}
