using System.ComponentModel.DataAnnotations.Schema;

namespace TweeterDB.Entity
{
    public class Tweet
    {
        public int TweetId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
