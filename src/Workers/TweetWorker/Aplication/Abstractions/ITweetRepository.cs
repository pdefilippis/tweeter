using TweetWorker.Entities;

namespace TweetWorker.Aplication.Abstractions
{
    public interface ITweetRepository
    {
        Task SaveTweet(TweetEntity tweet);
    }
}
