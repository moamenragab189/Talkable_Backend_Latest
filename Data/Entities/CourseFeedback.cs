using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talkable.Data.Entities
{
    public class CourseFeedback
    {
        [Key]
        public int Feedback_Id { get; set; }
        [ForeignKey("user")]
        public int User_Id { get; set; }
        [ForeignKey("Courses")]
        public int Course_Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }

        public DateTime Created_At { get; set; } = DateTime.UtcNow;
        public User user { get; set; }
        public Courses Courses { get; set; }
    }
}
