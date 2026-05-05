using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talkable.Data.Entities
{
    public class OTP
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationTime { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
