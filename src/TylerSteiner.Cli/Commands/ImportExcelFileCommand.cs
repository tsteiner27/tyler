using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TylerSteiner.Cli.CommandLine;
using TylerSteiner.Cli.EntityFramework;
using TylerSteiner.Models;
using TylerSteiner.Services;
using TylerSteiner.Services.Providers;

namespace TylerSteiner.Cli.Commands
{
    internal class ImportExcelFileCommand
    {
        public static Action<CommandLineApplication> Configure(IServiceProvider provider, CancellationToken cancel)
        {
            return command =>
            {
                command.Description = "Imports the excel file";
                command.HelpOption("-?|-h|--help");

                var path = command.Argument("Path", "CSV file path");

                command.OnExecute(async () => await provider.GetService<ImportExcelFileCommand>().ExecuteAsync(path.Value, cancel));
            };
        }

        private readonly IOmdbMovieDataService _omdbData;
        private readonly IImdbMovieDataService _imdbData;
        private readonly IMovieImportService _importService;
        private readonly ILogger<ImportExcelFileCommand> _logger;

        public ImportExcelFileCommand(IOmdbMovieDataService omdbData, IImdbMovieDataService imdbData, IMovieImportService importService, ILogger<ImportExcelFileCommand> logger)
        {
            _omdbData = omdbData;
            _imdbData = imdbData;
            _importService = importService;
            _logger = logger;
        }

        private async Task<int> ExecuteAsync(string path, CancellationToken token)
        {
            // Create lists to store objects
            var movies = new List<Movie>();
            var actors = new List<Actor>();
            var cinematographers = new List<Cinematographer>();
            var composers = new List<Composer>();
            var directors = new List<Director>();
            var distributors = new List<Distributor>();
            var genres = new List<Genre>();
            var producers = new List<Producer>();
            var studios = new List<Studio>();
            var writers = new List<Writer>();

            // Read in the tsv and convert to typed object
            var lines = File.ReadAllLines(path).Skip(1).Select(Parse);
            
            // Rankings in the file are truncated to one digit, with ties broken by Rank
            // We group by Rating, Order by Rank, and assign computed ratings so the 
            // new rankings are distributed uniformly.
            foreach (var ratingGroup in lines.GroupBy(l => l.Rating))
            {
                var numSameRating = ratingGroup.Count();
                var numerator = 1;
                var denominator = (numSameRating + 1) * 10.0;
                
                foreach (var line in ratingGroup.OrderBy(l => l.Rank))
                {
                    // Adjust the rating so they are all correctly ordered
                    var scoreCorrection = numerator/denominator;
                    var rating = line.Rating + scoreCorrection;

                    _logger.LogInformation("Importing '{Movie}' with adjusted rating {Rating}", line.Title, rating);

                    // Get movie data from OMDB
                    var omdb = await _omdbData.GetMovieDataAsync(line.Title);
                    if (omdb == null)
                    {
                        _logger.LogWarning("No OMDB movie data found for '{Movie}'", line.Title);
                        continue;
                    }
                    _logger.LogInformation("Matched '{Movie}' with data from IMDB ID '{Id}' ({OmdbMovie})", line.Title, omdb.ImdbId, omdb.Title);

                    // Get movie data from IMDB
                    var imdb = await _imdbData.GetMovieDataAsync(omdb.ImdbId);
                    if (imdb == null)
                    {
                        _logger.LogWarning("No IMDB movie data found for {Movie} '{Id}'", line.Title, omdb.ImdbId);
                        continue;
                    }
                    _logger.LogInformation("Pulled data from IMDB for IMDB ID '{Id}'", omdb.ImdbId);

                    // Add data we've parsed
                    var excelGenres = line.Genres.Where(g => !string.IsNullOrWhiteSpace(g)).Select(g => new Genre
                    {
                        Name = g,
                        Id = g.ToLower(),
                    }).Distinct(new ImdbEqualityComparer<Genre>()).ToList();

                    actors.AddRange(imdb.Actors);
                    cinematographers.AddRange(imdb.Cinematographers);
                    distributors.AddRange(imdb.Distributors);
                    directors.AddRange(imdb.Directors);
                    producers.AddRange(imdb.Producers);
                    composers.AddRange(imdb.Composers);
                    writers.AddRange(imdb.Writers);
                    studios.AddRange(imdb.Studios);
                    genres.AddRange(excelGenres);

                    var movie = new Movie
                    {
                        Title = omdb.Title,
                        Rating = rating,
                        Poster = omdb.Poster,
                        Id = omdb.ImdbId,
                        Budget = line.Budget.GetValueOrDefault(),
                        ImdbRating = line.ImdbRating,
                        Length = line.RunningTime,
                        MpaaRating = line.MpaaRating,
                        SawPremier = line.Premiere,
                        TimesWatched = line.TimesWatched,
                        TimesWatchedInTheater = line.TimesWatchedTheater,
                        UsBoxOffice = line.UsBoxOffice.GetValueOrDefault(),
                        WorldBoxOffice = line.WorldwideBoxOffice.GetValueOrDefault(),
                        Year = line.Year,

                        // Related properties
                        ActorMappings = imdb.Actors.Select(a => new ActorMapping { MovieId = omdb.ImdbId, ActorId = a.Id }).ToList(),
                        CinematographerMappings = imdb.Cinematographers.Select(c => new CinematographerMapping { MovieId = omdb.ImdbId, CinematographerId = c.Id }).ToList(),
                        DistributorMappings = imdb.Distributors.Select(d => new DistributorMapping { MovieId = omdb.ImdbId, DistributorId = d.Id }).ToList(),
                        DirectorMappings = imdb.Directors.Select(d => new DirectorMapping { MovieId = omdb.ImdbId, DirectorId = d.Id }).ToList(),
                        ProducerMappings = imdb.Producers.Select(p => new ProducerMapping { MovieId = omdb.ImdbId, ProducerId = p.Id }).ToList(),
                        ComposerMappings = imdb.Composers.Select(c => new ComposerMapping { MovieId = omdb.ImdbId, ComposerId = c.Id }).ToList(),
                        WriterMappings = imdb.Writers.Select(w => new WriterMapping { MovieId = omdb.ImdbId, WriterId = w.Id }).ToList(),
                        StudioMappings = imdb.Studios.Select(s => new StudioMapping { MovieId = omdb.ImdbId, StudioId = s.Id }).ToList(),
                        GenreMappings = excelGenres.Select(g => new GenreMapping { MovieId = omdb.ImdbId, GenreId = g.Id }).ToList(),
                    };
                    movies.Add(movie);

                    // Increment counter and continue
                    numerator ++;
                }
            }

            // Remove duplicates
            actors = actors.Distinct(new ImdbEqualityComparer<Actor>()).ToList();
            cinematographers = cinematographers.Distinct(new ImdbEqualityComparer<Cinematographer>()).ToList();
            composers = composers.Distinct(new ImdbEqualityComparer<Composer>()).ToList();
            directors = directors.Distinct(new ImdbEqualityComparer<Director>()).ToList();
            distributors = distributors.Distinct(new ImdbEqualityComparer<Distributor>()).ToList();
            genres = genres.Distinct(new ImdbEqualityComparer<Genre>()).ToList();
            producers = producers.Distinct(new ImdbEqualityComparer<Producer>()).ToList();
            studios = studios.Distinct(new ImdbEqualityComparer<Studio>()).ToList();
            writers = writers.Distinct(new ImdbEqualityComparer<Writer>()).ToList();

            // Insert entities into DB
            using (var context = new MoviesDbContext())
            {
                _logger.LogInformation("Starting context session");

                context.Actors.AddRange(actors);
                _logger.LogInformation("Attached Actors to context");

                context.Cinematographers.AddRange(cinematographers);
                _logger.LogInformation("Attached Cinematographers to context");

                context.Composers.AddRange(composers);
                _logger.LogInformation("Attached Composers to context");

                context.Directors.AddRange(directors);
                _logger.LogInformation("Attached Directors to context");

                context.Distributors.AddRange(distributors);
                _logger.LogInformation("Attached Distributors to context");

                context.Genres.AddRange(genres);
                _logger.LogInformation("Attached Genres to context");

                context.Producers.AddRange(producers);
                _logger.LogInformation("Attached Producers to context");

                context.Studios.AddRange(studios);
                _logger.LogInformation("Attached Studios to context");

                context.Writers.AddRange(writers);
                _logger.LogInformation("Attached Writers to context");

                await context.SaveChangesAsync(token);
                _logger.LogInformation("Saved changes");
            }

            foreach (var movie in movies)
            {
                using (var context = new MoviesDbContext())
                {
                    _logger.LogInformation("Attaching movie {Name}", movie.Title);
                    context.Movies.Add(movie);
                    await context.SaveChangesAsync(token);
                }
            }

            return 1;
        }

        private static ExcelFileLine Parse(string line)
        {
            var parts = line.Split('\t');

            var parsed = new ExcelFileLine();

            parsed.Rank = int.Parse(parts[0]);
            parsed.Title = parts[1];
            parsed.Rating = double.Parse(parts[2]);
            parsed.TimesWatched = int.Parse(parts[3]);
            parsed.Genres = new[]
            {
                parts[4], parts[5], parts[6], parts[7], parts[8], parts[9], parts[10],
            };
            parsed.MpaaRating = parts[11];
            parsed.RunningTime = int.Parse(parts[12]);
            parsed.TimesWatchedTheater = int.Parse(parts[13]);
            parsed.Premiere = !string.IsNullOrWhiteSpace(parts[14]);
            parsed.ImdbRating = double.Parse(parts[15]);
            parsed.Year = int.Parse(parts[17]);
            parsed.Actors = new[]
            {
                parts[18], parts[19], parts[20], parts[21], parts[22], parts[23], parts[24], parts[25], parts[25],
                parts[26], parts[27],
            };
            parsed.Actresses = new[]
            {
                parts[28], parts[29], parts[30], parts[31], parts[32], parts[33], parts[34], parts[35], parts[36],
            };
            parsed.Directors = new[]
            {
                parts[37], parts[38],
            };
            parsed.Writers = new[]
            {
                parts[39], parts[40], parts[41], parts[42],
            };
            parsed.Producers = new[]
            {
                parts[43], parts[44], parts[45], parts[46],
            };
            parsed.Distributors = new[]
            {
                parts[47], parts[48], parts[49],
            };
            parsed.Studios = new[]
            {
                parts[50], parts[51], parts[52], parts[53], parts[54], parts[55], parts[56], parts[57], parts[58],
                parts[59], parts[60],
            };
            parsed.Budget = ParseMoney(parts[61]);
            parsed.UsBoxOffice = ParseMoney(parts[62]);
            parsed.WorldwideBoxOffice = ParseMoney(parts[63]);
            parsed.Composers = new[]
            {
                parts[64], parts[65], parts[66],
            };
            parsed.Cinematographer = parts[67];

            return parsed;
        }

        private static int? ParseMoney(string item)
        {
            var clean = item
                .Replace("\"", "")
                .Replace(",", "")
                .Replace("$", "")
                .Trim();

            int money;
            if (int.TryParse(clean, out money))
            {
                return money;
            }

            return null;
        }

        private class ExcelFileLine
        {
            public int Rank { get; set; }
            public string Title { get; set; }
            public double Rating { get; set; }
            public int TimesWatched { get; set; }
            public string[] Genres { get; set; }
            public string MpaaRating { get; set; }
            public int RunningTime { get; set; }
            public int TimesWatchedTheater { get; set; }
            public bool Premiere { get; set; }
            public double ImdbRating { get; set; }
            public int Year { get; set; }
            public string[] Actors { get; set; }
            public string[] Actresses { get; set; }
            public string[] Directors { get; set; }
            public string[] Writers { get; set; }
            public string[] Producers { get; set; }
            public string[] Distributors { get; set; }
            public string[] Studios { get; set; }
            public int? Budget { get; set; }
            public int? UsBoxOffice { get; set; }
            public int? WorldwideBoxOffice { get; set; }
            public string[] Composers { get; set; }
            public string Cinematographer { get; set; }
        }
    }
}