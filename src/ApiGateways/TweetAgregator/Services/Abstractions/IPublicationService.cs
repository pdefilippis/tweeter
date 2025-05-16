using TweetAgregator.Model;

namespace TweetAgregator.Services.Abstractions
{
    public interface IPublicationService
    {
        Task<PublicationResponse?> SaveMessageAsync(int user, PublicationRequest publication);
    }
}
