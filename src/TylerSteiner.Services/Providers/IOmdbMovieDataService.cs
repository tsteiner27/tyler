using System.Threading.Tasks;
using TylerSteiner.Models.Providers;

namespace TylerSteiner.Services.Providers
{
    public interface IOmdbMovieDataService
    {
        Task<OmdbMovieData> GetMovieDataAsync(string title);
    }
}