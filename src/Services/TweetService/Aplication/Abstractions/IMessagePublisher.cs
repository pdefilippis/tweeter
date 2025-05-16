namespace TweetService.Aplication.Abstractions
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message) where T : class;
    }
}
