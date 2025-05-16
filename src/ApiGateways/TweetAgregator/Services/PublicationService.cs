using TweetAgregator.Model;
using TweetAgregator.Models.Builders;
using TweetAgregator.Services.Abstractions;
using TweetService;

namespace TweetAgregator.Service
{
    public class PublicationService(Publication.PublicationClient _publicationClient) : IPublicationService
    {
        public async Task<PublicationResponse?> SaveMessageAsync(int user, PublicationRequest publication)
        {
            var publicationReq = new MessageRequestBuilder().WithMessage(publication.Message).WithUserId(user);
            var resp = await _publicationClient.SaveMessageAsync(publicationReq.Build());

            return new PublicationResponse
            {
                Success = resp.Success,
            };
        }
    }
}
