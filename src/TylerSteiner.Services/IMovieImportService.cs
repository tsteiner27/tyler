using System.Collections.Generic;
using System.Threading.Tasks;
using TylerSteiner.Models;

namespace TylerSteiner.Services
{
    public interface IMovieImportService
    {
        Task ImportMovie(
            Movie movie,
            IEnumerable<Genre> genres,
            IEnumerable<Actor> actors,
            IEnumerable<Cinematographer> cinematographers,
            IEnumerable<Composer> composers,
            IEnumerable<Director> directors,
            IEnumerable<Distributor> distributors,
            IEnumerable<Producer> producers,
            IEnumerable<Studio> studios,
            IEnumerable<Writer> writers);
    }
}