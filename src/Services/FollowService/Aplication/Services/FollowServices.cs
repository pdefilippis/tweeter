using Grpc.Core;
using FollowService.Aplication.Abstractions;
using FollowService.Protos.Follow;

namespace FollowService.Aplication.Services
{
    public class FollowServices(IMessagePublisher _messagePublisher) : Follower.FollowerBase
    {
        public override async Task<FollowResponse> FollowSave(FollowRequest request, ServerCallContext context)
        {
            await _messagePublisher.PublishAsync(request);
            return new FollowResponse
            {
                Success = true,
            };
        }
    }
}
