using System.ComponentModel.DataAnnotations;

namespace Talkable.Data.Entities
{
    //many to many table
    public class UserCourses
    {
        [Key]
        public int UserCourses_Id { get; set; }
        public int User_Id { get; set; }
        
        public int Course_Id { get; set; }
        
        public int Progress { get; set; } = 0;
        public int Completed_Lessons { get; set; }= 0;


        public User Tbuser { get; set; }
        public Courses Tbcourse { get; set; }

    }
}
