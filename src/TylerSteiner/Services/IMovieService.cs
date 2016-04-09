using System.Collections.Generic;
using System.Threading.Tasks;
using TylerSteiner.Models;

namespace TylerSteiner.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetMoviesAsync();
    }
}