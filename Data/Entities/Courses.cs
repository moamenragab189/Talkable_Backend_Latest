using System.ComponentModel.DataAnnotations;

namespace Talkable.Data.Entities
{
    public class Courses
    {
        public Courses()
        {
            User_courses = new HashSet<UserCourses>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(400)]
        public string Description { get; set; }
        [Required]
        [MaxLength(400)]
        public string Instructor_Info { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Hours {  get; set; } = 0;

        public ICollection <UserCourses> User_courses { get; set; }
        public ICollection<CourseFeedback> User_Feedback { get; set; }
    }
}
