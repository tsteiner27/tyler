using System.Threading.Tasks;
using TylerSteiner.Models.Providers;

namespace TylerSteiner.Services.Providers
{
    public interface IImdbMovieDataService
    {
        Task<ImdbMovieData> GetMovieDataAsync(string imdbId);
    }
}