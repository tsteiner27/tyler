namespace TylerSteiner.Models
{
    public class ActorMapping
    {
        public Movie Movie { get; set; }
        public string MovieId { get; set; }

        public Actor Actor { get; set; }
        public string ActorId { get; set; }
    }

    public class CinematographerMapping
    {
        public Movie Movie { get; set; }
        public string MovieId { get; set; }

        public Cinematographer Cinematographer { get; set; }
        public string CinematographerId { get; set; }
    }

    public class ComposerMapping
    {
        public Movie Movie { get; set; }
        public string MovieId { get; set; }
        
        public Composer Composer { get; set; }
        public string ComposerId { get; set; }
    }

    public class DirectorMapping
    {
        public Movie Movie { get; set; }
        public string MovieId { get; set; }

        public Director Director { get; set; }
        public string DirectorId { get; set; }
    }

    public class DistributorMapping
    {
        public Movie Movie { get; set; }
        public string MovieId { get; set; }

        public Distributor Distributor { get; set; }
        public string DistributorId { get; set; }
    }

    public class GenreMapping
    {
        public Movie Movie { get; set; }
        public string MovieId { get; set; }

        public Genre Genre { get; set; }
        public string GenreId { get; set; }
    }

    public class ProducerMapping
    {
        public Movie Movie { get; set; }
        public string MovieId { get; set; }

        public Producer Producer { get; set; }
        public string ProducerId { get; set; }
    }

    public class StudioMapping
    {
        public Movie Movie { get; set; }
        public string MovieId { get; set; }

        public Studio Studio { get; set; }
        public string StudioId { get; set; }
    }

    public class WriterMapping
    {
        public Movie Movie { get; set; }
        public string MovieId { get; set; }

        public Writer Writer { get; set; }
        public string WriterId { get; set; }
    }
}