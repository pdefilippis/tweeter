namespace TweeterDB.Entity
{
    public class User
    {
        public User()
        {
            Followers = new HashSet<User>();
            Tweets = new HashSet<Tweet>();
        }

        public int UserId { get; set; }
        public string Name { get; set; }
        public string LasName { get; set; }
        public virtual ICollection<User> Followers { get; set; }
        public virtual ICollection<Tweet> Tweets { get; set; }
    }
}
