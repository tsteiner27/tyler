using System.Collections.Generic;

namespace TylerSteiner.Models.Providers
{
    public class ImdbMovieData : IImdbEntity
    {
        public string Id { get; set; }

        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<Producer> Producers { get; set; }
        public IEnumerable<Cinematographer> Cinematographers { get; set; }
        public IEnumerable<Director> Directors { get; set; }
        public IEnumerable<Composer> Composers { get; set; }
        public IEnumerable<Distributor> Distributors { get; set; }
        public IEnumerable<Studio> Studios { get; set; }
        public IEnumerable<Writer> Writers { get; set; }   
    }
}