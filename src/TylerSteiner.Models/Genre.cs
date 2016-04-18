using System.Collections.Generic;

namespace TylerSteiner.Models
{
    public class Genre : IImdbEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<GenreMapping> GenreMappings { get; set; }
    }
}