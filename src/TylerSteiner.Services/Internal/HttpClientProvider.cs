using System;
using System.Net.Http;

namespace TylerSteiner.Services.Internal
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly Lazy<HttpClient> _client = new Lazy<HttpClient>(() => new HttpClient());
        
        public HttpClient Get()
        {
            return _client.Value;
        }
    }
}