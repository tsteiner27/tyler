using System.Collections.Generic;

namespace TylerSteiner.Models
{
    public class Director : IImdbEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<DirectorMapping> DirectorMappings { get; set; }
    }
}