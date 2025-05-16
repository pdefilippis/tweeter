using TimelineService.Models;

namespace TimelineService.Aplication.Abstraction
{
    public interface ITweetRepository
    {
        Task<TweetTimelineHeader> GetTweets(TweetTimelineFilter filter);
    }
}
