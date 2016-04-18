namespace TylerSteiner.Services.Internal
{
    public interface IJsonParser
    {
        T Deserialize<T>(string json);
    }
}