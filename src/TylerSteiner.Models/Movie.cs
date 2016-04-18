namespace TylerSteiner.Models
{
    public class Movie : IEntity, IImdbEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public double? Rating { get; set; }
        public string ImdbId { get; set; }
        public string Poster { get; set; }
        public int TimesWatched { get; set; }
        public string MpaaRating { get; set; }
        public int Length { get; set; }
        public int TimesWatchedInTheater { get; set; }
        public bool SawPremier { get; set; }
        public double ImdbRating { get; set; }
        public int Year { get; set; }
        public long Budget { get; set; }
        public long UsBoxOffice { get; set; }
        public long WorldBoxOffice { get; set; }
    }
}