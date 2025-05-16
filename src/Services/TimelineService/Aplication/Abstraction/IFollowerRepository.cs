namespace TimelineService.Aplication.Abstraction
{
    public interface IFollowerRepository
    {
        Task<IList<int>> GetFollowerByUser(int userId);
    }
}
