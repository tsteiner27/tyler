using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.OptionsModel;
using TylerSteiner.Core.Options;

namespace TylerSteiner.Services.Data
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly DatabaseConfig _options;

        public SqlConnectionFactory(IOptions<DatabaseConfig> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public async Task<TResult> UseAsync<TResult>(Func<DbConnection, Task<TResult>> action)
        {
            using (var connection = new SqlConnection(_options.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    return await action(connection);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}