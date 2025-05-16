using CacheService.Abstractions;
using Grpc.Core;
using TimelineService.Aplication.Abstraction;
using TimelineService.Aplication.Builders;
using TimelineService.Models;

namespace TimelineService.Aplication.Service
{
    public class TimelineService(ICacheService cacheService, IFollowerRepository followerRepository, ITweetRepository tweetRepository) : TweetsTimeline.TweetsTimelineBase
    {
        //TODO: Podemos crear una variable de configuracion en appsettings.json
        const int expiredCacheMin = 1;
        public override async Task<TweetsHeaderFilteredResponse> GetTweets(FilterTweetsRequest request, ServerCallContext context)
        {
            var cacheKey = $"GetTweets-{request.UserId}-{request.PageSize}-{request.Page}";
            var dataCache = await cacheService.GetAsync<TweetsHeaderFilteredResponse>(cacheKey);
            if (dataCache != null)
                return dataCache;
            

            var filterUser = await GetFilterByUsers(request.UserId);
            var data = await GetTweets(request, filterUser);

            var tweetHeaderResp = new TweetsHeaderFilteredResponseBuilder()
                .WithFilterTweets(request)
                .WithTweetTimeline(data);

            var resp = tweetHeaderResp.Build();

            await cacheService.SetAsync(cacheKey, resp, TimeSpan.FromMinutes(expiredCacheMin));
            return resp;

        }

        public async Task<TweetTimelineHeader> GetTweets(FilterTweetsRequest request, IList<int> filterUsers)
        {
            var filter = new TweetTimelineFilter
            {
                Page = request.Page,
                PageSize = request.PageSize,
                UserIds = filterUsers
            };

            return await tweetRepository.GetTweets(filter);
        }

        public async Task<IList<int>> GetFilterByUsers(int userId)
        {
            var filterUser = await followerRepository.GetFollowerByUser(userId);
            filterUser.Add(userId);

            return filterUser;
        }

    }
}
