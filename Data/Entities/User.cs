using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.EntityFrameworkCore;
namespace Talkable.Data.Entities
{
    public class User
    {
        public User()
        {
            User_Courses =new HashSet<UserCourses>();
        }
        [Key]
        public int User_Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string First_Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Last_Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public UserType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<UserCourses>? User_Courses { get; set; }
        public ICollection<CourseFeedback>? User_Feedback { get; set; }
        public OTP? User_OTP { get; set; }

    }
    public enum UserType
    {
        deaf,
        normal,
    }
}
