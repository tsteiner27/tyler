namespace TylerSteiner.Models
{
    public class Composer : IEntity, IImdbEntity
    {
        public long Id { get; set; }
        public string ImdbId { get; set; }
        public string Name { get; set; }
    }
}