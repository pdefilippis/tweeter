using FollowAgregator.Model.Builders;
using FollowAgregator.Services.Abstractions;
using FollowService.Protos.Follow;

namespace FollowAgregator.Services
{
    public class FollowerService(Follower.FollowerClient followerClient) : IFollowService
    {
        public async Task<Model.FollowResponse?> SavFollowAsync(int user, Model.FollowRequest follow)
        {
            var followRequest = new FollowRequestBuilder()
                .WithUser(user)
                .WithFollowTo(follow.FollowTo);

            var resp = await followerClient.FollowSaveAsync(followRequest.Build());
            return new Model.FollowResponse
            {
                Success = resp.Success
            };
        }
    }
}
