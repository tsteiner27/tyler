using System.Collections.Generic;

namespace TylerSteiner.Models
{
    public class Writer : IImdbEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<WriterMapping> WriterMappings { get; set; }
    }
}