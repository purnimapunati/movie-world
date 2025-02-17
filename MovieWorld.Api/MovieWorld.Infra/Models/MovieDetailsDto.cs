namespace MovieWorld.Service.Models
{
    public class MovieDetailsDto
    {
        public string ID { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Rating { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Poster { get; set; } = string.Empty;
    }
}
