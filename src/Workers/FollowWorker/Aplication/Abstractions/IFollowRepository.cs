using FollowWorker.Domains;

namespace FollowWorker.Aplication.Abstractions
{
    public interface IFollowRepository
    {
        Task SaveFollow(FollowEntity follow);
    }
}
