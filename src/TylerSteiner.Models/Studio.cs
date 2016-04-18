namespace TylerSteiner.Models
{
    public class Studio : IEntity, IImdbEntity
    {
        public long Id { get; set; }
        public string ImdbId { get; set; }
        public string Name { get; set; }
    }
}