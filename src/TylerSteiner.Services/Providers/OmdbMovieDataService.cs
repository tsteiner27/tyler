using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TylerSteiner.Models.Providers;
using TylerSteiner.Services.Internal;

namespace TylerSteiner.Services.Providers
{
    public class OmdbMovieDataService : IOmdbMovieDataService
    {
        private const string ApiFormat = "http://www.omdbapi.com/?t={0}&y=&plot=short&r=json";

        private readonly ILogger<OmdbMovieDataService> _logger;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IJsonParser _jsonParser;

        public OmdbMovieDataService(ILogger<OmdbMovieDataService> logger, IHttpClientProvider httpClientProvider, IJsonParser jsonParser)
        {
            _logger = logger;
            _httpClientProvider = httpClientProvider;
            _jsonParser = jsonParser;
        }

        public async Task<OmdbMovieData> GetMovieDataAsync(string title)
        {
            try
            {
                var encoded = WebUtility.UrlEncode(title);
                var url = string.Format(ApiFormat, encoded);

                var http = _httpClientProvider.Get();
                var response = await http.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                var data = _jsonParser.Deserialize<OmdbMovieData>(json);

                return data;
            }
            catch (Exception e)
            {
                _logger.LogCritical("Failed to retrieve OMDB data", e);
                return null;
            }
        }
    }
}