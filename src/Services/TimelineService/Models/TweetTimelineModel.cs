namespace TimelineService.Models
{
    public class TweetTimelineFilter
    {
        public IList<int> UserIds { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class TweetTimelineHeader
    {
        public int TotalRows { get; set; }
        public IList<TweetTimelineFiltered> Tweets { get; set; }
    }

    public class TweetTimelineFiltered 
    {
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}
