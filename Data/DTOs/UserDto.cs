using System.ComponentModel.DataAnnotations;
using Talkable.Data.Entities;

namespace Talkable.Data.DTOs
{
    public class UserDto
    {
        public int User_Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Full_Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public UserType Type { get; set; }
    }
}
