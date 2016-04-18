using System.Collections.Generic;

namespace TylerSteiner.Models
{
    public class Distributor : IImdbEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<DistributorMapping> DistributorMappings { get; set; }
    }
}