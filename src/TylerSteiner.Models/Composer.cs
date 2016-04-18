using System.Collections.Generic;

namespace TylerSteiner.Models
{
    public class Composer : IImdbEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<ComposerMapping> ComposerMappings { get; set; }
    }
}