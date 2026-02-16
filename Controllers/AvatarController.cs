using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talkable.Data;
using Talkable.Services;

namespace Talkable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private readonly AvatarService _avatarService;
        public AvatarController(AvatarService avatarService)
        {
            _avatarService = avatarService;
        }
        [HttpPost]
        public async Task<IActionResult> GetAction([FromBody]name name)
        {
            string word = name.Name.Trim();
            if (string.IsNullOrEmpty(word))
            {
                return BadRequest("Word is required.");
            }
          var animation_path= await _avatarService.GetAction(word);
                if (animation_path == null)
                {
                    return NotFound("No animation found for the given word.");
                }
            return Ok(new
            {
                url = $"{Request.Scheme}://{Request.Host}{animation_path}"
            });
            ;
        }
    }
}
