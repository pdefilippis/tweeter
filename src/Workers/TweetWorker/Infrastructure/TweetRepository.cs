using TweeterDB.Context;
using TweeterDB.Entity;
using TweetWorker.Aplication.Abstractions;
using TweetWorker.Entities;

namespace TweetWorker.Infrastructure
{
    public class TweetRepository(TweetContext context) : ITweetRepository
    {
        public async Task SaveTweet(TweetEntity tweet)
        {
            var tweetModel = new Tweet
            {
                Message = tweet.Message,
                UserId = tweet.UserId,
            };

            await context.Tweets.AddAsync(tweetModel);
            await context.SaveChangesAsync();
        }


    }
}
