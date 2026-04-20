namespace Talkable.Data.Models
{
    public class Room
    {
        public string Id { get; set; }
        public string? FirstUserId { get; set; }
        public string? SecondUserId { get; set; }
        public string? FirstUserConnectionId { get; set; }
        public string? SecondUserConnectionId { get; set; }
    }
}
