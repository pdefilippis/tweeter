using Microsoft.EntityFrameworkCore;
using TimelineService.Aplication.Abstraction;
using TweeterDB.Context;

namespace TimelineService.Infrastructure
{
    public class FollowerRepository(TweetContext context) : IFollowerRepository
    {
        public async Task<IList<int>> GetFollowerByUser(int userId)
        {
            var user = await context.Users.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (user == null)
                throw new InvalidOperationException("No se encontro el usuario");

            return user.Followers.Select(x => x.UserId).ToList();
        }
    }
}
