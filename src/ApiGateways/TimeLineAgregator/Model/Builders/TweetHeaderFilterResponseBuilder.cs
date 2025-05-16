using TimelineService;

namespace TimeLineAgregator.Model.Builders
{
    public class TweetHeaderFilterResponseBuilder
    {
        private TweetsHeaderFilteredResponse _response;
        public TweetHeaderFilterResponseBuilder WithHeader(TweetsHeaderFilteredResponse response)
        {
            _response = response;
            return this;
        }

        public TweetHeaderFilterResponse Build()
        {
            return new TweetHeaderFilterResponse
            {
                CurrentPage = _response.CurrentPage,
                PageSize = _response.PageSize,
                HasNextPage = _response.HasNextPage,
                Tweets = _response.Tweets.Select(x => new TweetFilterResponse
                {
                    Message = x.Message,
                    UserId = x.UserId,
                }).ToList()
            };
        }
    }
}
