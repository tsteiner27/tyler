using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using TylerSteiner.Core.Extensions;
using TylerSteiner.Models;
using TylerSteiner.Services.Data;
using TylerSteiner.Services.Internal;

namespace TylerSteiner.Services
{
    public class MovieImportService : IMovieImportService
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryFactory _sqlFactory;
        private readonly ILogger<MovieImportService> _logger;

        private readonly IRelatedTypeImporter<Genre> _genreImporter;
        private readonly IRelatedTypeImporter<Actor> _actorImporter;
        private readonly IRelatedTypeImporter<Cinematographer> _cinematographerImporter;
        private readonly IRelatedTypeImporter<Composer> _composerImporter;
        private readonly IRelatedTypeImporter<Director> _directorImporter;
        private readonly IRelatedTypeImporter<Distributor> _distributorImporter;
        private readonly IRelatedTypeImporter<Producer> _producerImporter;
        private readonly IRelatedTypeImporter<Studio> _studioImporter;
        private readonly IRelatedTypeImporter<Writer> _writerImporter;

        public MovieImportService(
            IDbConnectionFactory connectionFactory, 
            ISqlQueryFactory sqlFactory, 
            ILogger<MovieImportService> logger,
            ILoggerFactory loggerFactory)
        {
            _connectionFactory = connectionFactory;
            _sqlFactory = sqlFactory;
            _logger = logger;

            _genreImporter = new RelatedTypeImporter<Genre>(sqlFactory, loggerFactory);
            _actorImporter = new RelatedTypeImporter<Actor>(sqlFactory, loggerFactory);
            _cinematographerImporter = new RelatedTypeImporter<Cinematographer>(sqlFactory, loggerFactory);
            _composerImporter = new RelatedTypeImporter<Composer>(sqlFactory, loggerFactory);
            _directorImporter = new RelatedTypeImporter<Director>(sqlFactory, loggerFactory);
            _distributorImporter = new RelatedTypeImporter<Distributor>(sqlFactory, loggerFactory);
            _producerImporter = new RelatedTypeImporter<Producer>(sqlFactory, loggerFactory);
            _studioImporter = new RelatedTypeImporter<Studio>(sqlFactory, loggerFactory);
            _writerImporter = new RelatedTypeImporter<Writer>(sqlFactory, loggerFactory);
        }

        public async Task ImportMovie(
            Movie movie, 
            IEnumerable<Genre> genres, 
            IEnumerable<Actor> actors, 
            IEnumerable<Cinematographer> cinematographers,
            IEnumerable<Composer> composers, 
            IEnumerable<Director> directors, 
            IEnumerable<Distributor> distributors, 
            IEnumerable<Producer> producers, 
            IEnumerable<Studio> studios,
            IEnumerable<Writer> writers)
        {
            _logger.LogInformation("Importing {Movie} into the database.", movie.Title);
            
            await _connectionFactory.UseAsync(async connection =>
            {
                var insertMovie = _sqlFactory.CreateParameterizedInsertStatement<Movie>("Movies");

                var result = await connection.QueryAsync<string>(insertMovie, movie);
                var movieId = result.Single();

                _logger.LogInformation("{Movie} imported with Id '{Id}'", movie.Title, movieId);

                await _genreImporter.Import(connection, genres, movieId);
                await _actorImporter.Import(connection, actors, movieId);
                await _cinematographerImporter.Import(connection, cinematographers, movieId);
                await _composerImporter.Import(connection, composers, movieId);
                await _directorImporter.Import(connection, directors, movieId);
                await _distributorImporter.Import(connection, distributors, movieId);
                await _producerImporter.Import(connection, producers, movieId);
                await _studioImporter.Import(connection, studios, movieId);
                await _writerImporter.Import(connection, writers, movieId);

                return movieId;
            });
        }
        
        private interface IRelatedTypeImporter<in T> where T : class, IImdbEntity
        {
            Task Import(IDbConnection connection, IEnumerable<T> source, string movieId);
        }

        private class RelatedTypeImporter<T> : IRelatedTypeImporter<T> where T : class, IImdbEntity
        {
            private readonly ISqlQueryFactory _sqlFactory;
            private readonly ILogger _logger;

            private readonly string _elementName;
            private readonly string _tableName;
            private readonly string _mappingTableName;

            private IList<T> _cache;

            public RelatedTypeImporter(ISqlQueryFactory sqlFactory, ILoggerFactory loggerFactory)
            {
                _sqlFactory = sqlFactory;
                _logger = loggerFactory.CreateLogger<RelatedTypeImporter<T>>();

                var type = typeof (T);
                _elementName = type.Name;
                _tableName = $"{type.Name}s";
                _mappingTableName = $"{type.Name}Mapping";
            } 

            public async Task Import(IDbConnection connection, IEnumerable<T> source, string movieId)
            {
                if (_cache == null)
                {
                    _logger.LogInformation("Cache is null. Querying remote database.");
                    _cache = await connection.QueryAsync<T>($"SELECT * FROM {_tableName};").ToListAsync();
                    _logger.LogInformation("Loaded cache");
                }

                if (source == null)
                {
                    return;
                }

                var list = source.ToList();

                var missing = list
                    .Where(item => _cache.FirstOrDefault(cached => item.Id == cached.Id) == null)
                    .ToList();

                foreach (var item in missing)
                {
                    var sql = _sqlFactory.CreateParameterizedInsertStatement<T>(_tableName);
                    var result = await connection.QueryAsync<string>(sql, item);
                    var insertedId = result.Single();
                    _logger.LogInformation("Inserted {Id} in {Table}", item.Id, _tableName);

                    item.Id = insertedId;
                    _cache.Add(item);
                }

                foreach (var item in list)
                {
                    // Get the cached version, because we have an Id
                    var cached = _cache.First(g => g.Id == item.Id);

                    var sql = $"INSERT INTO {_mappingTableName} ([MovieId],[{_elementName}Id]) VALUES (@MovieId,@Id)";
                    await connection.ExecuteAsync(sql, new { MovieId = movieId, ImdbId = cached.Id });

                    _logger.LogInformation("Inserted {Id} in {Table}", item.Id, _mappingTableName);
                }
            }
        }
    }
}