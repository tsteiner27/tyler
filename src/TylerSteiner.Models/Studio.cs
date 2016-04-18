using System.Collections.Generic;

namespace TylerSteiner.Models
{
    public class Studio : IImdbEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<StudioMapping> StudioMappings { get; set; }
    }
}