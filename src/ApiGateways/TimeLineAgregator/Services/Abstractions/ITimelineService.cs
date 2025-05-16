using TimeLineAgregator.Model;

namespace TimeLineAgregator.Services.Abstractions
{
    public interface ITimelineService
    {
        Task<TweetHeaderFilterResponse> GetTimeline(int userId, TweetFilterRequest filter);
    }
}
