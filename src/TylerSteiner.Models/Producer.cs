using System.Collections.Generic;

namespace TylerSteiner.Models
{
    public class Producer : IImdbEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<ProducerMapping> ProducerMappings { get; set; }
    }
}