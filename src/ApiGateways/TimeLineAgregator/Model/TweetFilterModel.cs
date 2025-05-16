namespace TimeLineAgregator.Model
{
    public class TweetFilterRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }

    public class TweetHeaderFilterResponse
    {
        public int CurrentPage { get; set; }
        public bool HasNextPage { get; set; }
        public int PageSize { get; set; }
        public List<TweetFilterResponse> Tweets { get; set; } = new List<TweetFilterResponse>();
    }

    public class TweetFilterResponse
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
