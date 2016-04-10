using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TylerSteiner.Services;

namespace TylerSteiner.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;

        public HomeController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("movies")]
        public async Task<IActionResult> Movies()
        {
            var movies = await _movieService.GetMoviesAsync();
            return View(movies);
        }
    }
}