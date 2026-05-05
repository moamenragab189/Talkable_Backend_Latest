using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talkable.Services;

namespace Talkable.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private readonly AvatarService _avatarService;
        public AvatarController(AvatarService avatarService)
        {
            _avatarService = avatarService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAction(string word)
        {
            if (string.IsNullOrEmpty(word))
                return BadRequest("Word is required.");

            var animation_path = await _avatarService.GetAction(word);

            if (animation_path == null)
                return NotFound("No animation found for the given word.");

            // رجّع Object فيه url
            return Ok(new { url = animation_path });
        }
    }
}