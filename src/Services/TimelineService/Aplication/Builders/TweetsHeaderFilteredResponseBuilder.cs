using TimelineService.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TimelineService.Aplication.Builders
{
    public class TweetsHeaderFilteredResponseBuilder
    {
        private FilterTweetsRequest _filterTweetsRequest;
        private TweetTimelineHeader _tweetTimelineHeader;

        public TweetsHeaderFilteredResponseBuilder WithFilterTweets(FilterTweetsRequest filterTweetsRequest)
        {
            _filterTweetsRequest = filterTweetsRequest;
            return this;
        }

        public TweetsHeaderFilteredResponseBuilder WithTweetTimeline(TweetTimelineHeader tweetTimelineHeader)
        { 
            _tweetTimelineHeader = tweetTimelineHeader;
            return this;
        }

        public TweetsHeaderFilteredResponse Build()
        {
            if (_filterTweetsRequest == null || _tweetTimelineHeader == null)
                throw new ArgumentNullException("No es posible crear la respuesta con TweetsHeaderFilteredResponse");

            var resp = new TweetsHeaderFilteredResponse
            {
                CurrentPage = _filterTweetsRequest.Page,
                PageSize = _filterTweetsRequest.PageSize,
                UserId = _filterTweetsRequest.UserId,
                HasNextPage = _tweetTimelineHeader.TotalRows > _filterTweetsRequest.PageSize * _filterTweetsRequest.Page
            };

            var tweets = _tweetTimelineHeader.Tweets.Select(x => new TweetsFilteredResponse { 
                UserId = x.UserId, 
                Message = x.Message 
            });

            resp.Tweets.AddRange(tweets);

            return resp;
        }

    }
}
