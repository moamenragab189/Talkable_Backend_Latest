// -------------------------- Mg13 -----------------------


using System.ComponentModel.DataAnnotations;
namespace Talkable.Data.Entities
{
    public class Signs
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }  

        [Required]
        public string AnimationPath { get; set; } // path of animation file .glb
    }
}
