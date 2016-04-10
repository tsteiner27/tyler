namespace TylerSteiner.Models
{
    public class Movie
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double? Rating { get; set; }
        public string PosterUrl { get; set; }
    }
}