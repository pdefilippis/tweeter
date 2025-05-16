namespace FollowAgregator.Model.Builders
{
    public class FollowRequestBuilder
    {
        private int _followTo;
        private int _user;

        public FollowRequestBuilder WithFollowTo(int followTo)
        {
            _followTo = followTo;
            return this;
        }

        public FollowRequestBuilder WithUser(int user) 
        { 
            _user = user;
            return this;
        }

        public FollowService.Protos.Follow.FollowRequest Build()
        {
            return new FollowService.Protos.Follow.FollowRequest
            { 
                FollowTo = _followTo,
                UserId = _user
            };
        }
    }
}
