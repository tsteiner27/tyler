using System.Collections.Generic;

namespace TylerSteiner.Models
{
    public class Cinematographer : IImdbEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<CinematographerMapping> CinematographerMappings { get; set; }
    }
}