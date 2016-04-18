using Newtonsoft.Json;

namespace TylerSteiner.Services.Internal
{
    public class JsonParser : IJsonParser
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}