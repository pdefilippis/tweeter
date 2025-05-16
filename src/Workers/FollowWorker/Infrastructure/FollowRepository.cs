using FollowWorker.Aplication.Abstractions;
using FollowWorker.Domains;
using Microsoft.EntityFrameworkCore;
using TweeterDB.Context;
using TweeterDB.Entity;

namespace FollowWorker.Infrastructure
{
    public class FollowRepository(TweetContext context) : IFollowRepository
    {
        public async Task SaveFollow(FollowEntity follow)
        {
            var user = await context.Users.Where(x => x.UserId == follow.FollowTo).FirstOrDefaultAsync();
            var follower = await context.Users.Where(x => x.UserId == follow.UserId).FirstOrDefaultAsync();

            ValidateOperation(user, follower);

            user.Followers.Add(follower);
            await context.SaveChangesAsync();
        }

        private void ValidateOperation(User user, User follower)
        {
            if (user == null || follower == null)
                throw new InvalidOperationException("No es posible encontrar los usuarios");
        }
    }
}
