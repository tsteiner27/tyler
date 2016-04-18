using System.Net.Http;

namespace TylerSteiner.Services.Internal
{
    public interface IHttpClientProvider
    {
        HttpClient Get();
    }
}