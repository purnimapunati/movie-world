namespace MovieWorld.Infra.Models
{
    public class CacheItem
    {
        public MovieDetailsDto MovieDetails { get; set; }
        public DateTime CachedAt { get; set; }

        public bool IsExpired(TimeSpan expirationTime)
        {
            return DateTime.UtcNow - CachedAt > expirationTime;
        }
    }
}
