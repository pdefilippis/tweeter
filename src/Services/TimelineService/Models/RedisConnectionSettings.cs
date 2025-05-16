namespace TimelineService.Models
{
    public class RedisConnectionSettings
    {
        public string? User { get; set; }
        public string? Password { get; set; }
        public bool AbortOnConnectFail { get; set; }
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
