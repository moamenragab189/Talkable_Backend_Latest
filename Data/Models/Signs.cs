using System.ComponentModel.DataAnnotations;

namespace Talkable.Data.Models
{
    public class Signs
    {
        [Key]
        public int Sign_id { get; set; }
        [MaxLength(100)]
        public string name_ar { get; set; }
        [MaxLength(100)]
        public string name_en { get; set; }
        public sign_type type {  get; set; }
        public string Video_path { get; set; }
        public string Animation_path { get; set; }
        public int Duration { get; set; }
        public int Fps { get; set; }
    }
    public enum sign_type
    {
        letter,
        word,
        sentence
    }
}
