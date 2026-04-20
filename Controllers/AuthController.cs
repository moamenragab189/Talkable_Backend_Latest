using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talkable.Data.DTOs;
using Talkable.Data.Entities;
using Talkable.Data.Models;
using Talkable.Services;

namespace Talkable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;
        public AuthController(AuthService authService, JwtService jwtService,IMapper mapper)
        {
            _authService = authService;
            _jwtService = jwtService;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = _mapper.Map<User>(userDto);
            await _authService.register(user);
            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] usercred usercred)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _authService.login(usercred.email, usercred.password);
            if (user == null)
            {
                return Unauthorized();
            }
            var claims = _jwtService.AddUserClaims(user.Email, user.Type, user.User_Id);
            var token = _jwtService.CreateToken(claims);
            return Ok(new { AccessToken = token });
        }
    }
}