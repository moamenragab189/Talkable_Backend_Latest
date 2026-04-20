using System.ComponentModel.DataAnnotations;

namespace Talkable.Data.Models
{
    public class usercred
    {
        [Required]
        public string email { set; get; }
        [Required]
        public string password { set; get; }
    }
}
