namespace TylerSteiner.Models
{
    public class Genre : IEntity, IImdbEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImdbId { get; set; }
    }
}