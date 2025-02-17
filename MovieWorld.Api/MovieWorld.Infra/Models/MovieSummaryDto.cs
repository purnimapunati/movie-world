namespace MovieWorld.Infra.Models
{
    public class MovieSummaryDto
    {
        public string Title { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string ID { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Poster { get; set; } = string.Empty;
    }
    public class MovieList
    {
        public List<MovieSummaryDto> Movies { get; set; }
    }
}
