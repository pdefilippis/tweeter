using TweetService;

namespace TweetAgregator.Models.Builders
{
    public class MessageRequestBuilder
    {
        private string _message;
        private int _userId;

        public MessageRequestBuilder WithMessage(string message)
        {
            _message = message;
            return this;
        }

        public MessageRequestBuilder WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        public MessageRequest Build()
        {
            return new MessageRequest
            {
                Message = _message,
                UserId = _userId
            };
        }
    }
}
