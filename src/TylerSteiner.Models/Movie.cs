using System.Collections.Generic;

namespace TylerSteiner.Models
{
    public class Movie : IImdbEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }
        public string Poster { get; set; }
        public int TimesWatched { get; set; }
        public string MpaaRating { get; set; }
        public int Length { get; set; }
        public int TimesWatchedInTheater { get; set; }
        public bool SawPremier { get; set; }
        public double? ImdbRating { get; set; }
        public int Year { get; set; }
        public long? Budget { get; set; }
        public long? UsBoxOffice { get; set; }
        public long? WorldBoxOffice { get; set; }

        public ICollection<ActorMapping> ActorMappings { get; set; }
        public ICollection<CinematographerMapping> CinematographerMappings { get; set; }
        public ICollection<ComposerMapping> ComposerMappings { get; set; }
        public ICollection<DirectorMapping> DirectorMappings { get; set; }
        public ICollection<DistributorMapping> DistributorMappings { get; set; }
        public ICollection<GenreMapping> GenreMappings { get; set; }
        public ICollection<ProducerMapping> ProducerMappings { get; set; }
        public ICollection<StudioMapping> StudioMappings { get; set; }
        public ICollection<WriterMapping> WriterMappings { get; set; }
    }
}