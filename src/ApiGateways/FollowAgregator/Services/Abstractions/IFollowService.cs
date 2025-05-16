using FollowAgregator.Model;

namespace FollowAgregator.Services.Abstractions
{
    public interface IFollowService
    {
        Task<FollowResponse?> SavFollowAsync(int user, FollowRequest follow);
    }
}
