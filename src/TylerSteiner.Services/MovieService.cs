using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using TylerSteiner.Models;
using TylerSteiner.Services.Data;

namespace TylerSteiner.Services
{
    public class MovieService : IMovieService
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public MovieService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            return await _connectionFactory.UseAsync(async connection =>
                await connection.QueryAsync<Movie>("SELECT * FROM Movies"));
        }
    }
}