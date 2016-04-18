using System.Collections.Generic;

namespace TylerSteiner.Models
{
    public class Actor : IImdbEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<ActorMapping> ActorMappings { get; set; } 
    }
}