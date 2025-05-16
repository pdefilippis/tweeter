using TimeLineAgregator.Model;
using TimeLineAgregator.Model.Builders;
using TimeLineAgregator.Services.Abstractions;
using TimelineService;

namespace TimeLineAgregator.Services
{
    public class TimelineService(TweetsTimeline.TweetsTimelineClient client) : ITimelineService
    {
        public async Task<TweetHeaderFilterResponse> GetTimeline(int userId, TweetFilterRequest filter)
        {
            var filterTweets = new FilterTweetsRequestBuilder()
                .WithUserId(userId)
                .WithPage(filter.Page)
                .WithPageSize(filter.PageSize);

            var data = await client.GetTweetsAsync(filterTweets.Build());
            return new TweetHeaderFilterResponseBuilder().WithHeader(data).Build();
        }
    }
}
