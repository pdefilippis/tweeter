namespace TweetWorker.Entities
{
    public class TweetEntity
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }
        public DateTime Created { get; set; }
    }
}
