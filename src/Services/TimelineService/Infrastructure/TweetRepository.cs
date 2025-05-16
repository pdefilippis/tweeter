using Microsoft.EntityFrameworkCore;
using TimelineService.Aplication.Abstraction;
using TimelineService.Models;
using TweeterDB.Context;


namespace TimelineService.Infrastructure
{
    public class TweetRepository(TweetContext context) : ITweetRepository
    {
        public async Task<TweetTimelineHeader> GetTweets(TweetTimelineFilter filter)
        {
            var query = context.Tweets.AsNoTracking();
            query = query.Where(x => filter.UserIds.Contains(x.UserId))
                .OrderByDescending(x => x.CreatedAt);

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize).ToListAsync();

            //TODO: Se podria utilizar Automapper
            return new TweetTimelineHeader
            {
                TotalRows = totalCount,
                Tweets = data.Select(x => new TweetTimelineFiltered
                {
                    CreatedAt = x.CreatedAt,
                    Message = x.Message,
                    UserId = x.UserId
                }).ToList(),
            };
        }
    }
}
