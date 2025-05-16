using TimelineService;

namespace TimeLineAgregator.Model.Builders
{
    public class FilterTweetsRequestBuilder
    {
        private int _userId;
        private int _pageSize;
        private int _page;

        public FilterTweetsRequestBuilder WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        public FilterTweetsRequestBuilder WithPageSize(int pageSize)
        {
            _pageSize = pageSize;
            return this;
        }

        public FilterTweetsRequestBuilder WithPage(int page)
        {
            _page = page;
            return this;
        }

        public FilterTweetsRequest Build()
        {
            return new FilterTweetsRequest
            {
                Page = _page,
                UserId = _userId,
                PageSize = _pageSize
            };
        }
    }
}
