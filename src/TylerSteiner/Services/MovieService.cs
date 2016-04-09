using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using TylerSteiner.Models;

namespace TylerSteiner.Services
{
    public class MovieService : IMovieService
    {
        private readonly IConfiguration _config;

        public MovieService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            var connectionString = _config["Database:ConnectionString"];
            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    return await connection.QueryAsync<Movie>("SELECT * FROM Movies");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}