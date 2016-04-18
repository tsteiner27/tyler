using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TylerSteiner.Services.Internal
{
    public class SqlQueryFactory : ISqlQueryFactory
    {
        private readonly Dictionary<Type, string> _cachedInserts;
        private readonly ILogger<SqlQueryFactory> _logger;

        public SqlQueryFactory(ILogger<SqlQueryFactory> logger)
        {
            _cachedInserts = new Dictionary<Type, string>();
            _logger = logger;
        }

        public string CreateParameterizedInsertStatement<T>(string tableName)
        {
            var type = typeof (T);

            string cached;
            if (_cachedInserts.TryGetValue(type, out cached))
            {
                return cached;
            }

            var properties = type.GetProperties();
            var names = properties.Select(p => p.Name).Where(n => n != "Id").ToArray();

            var columns = string.Join(",", names.Select(name => $"[{name}]"));
            var values = string.Join(",", names.Select(name => $"@{name}"));

            var sql = $"INSERT INTO [dbo].[{tableName}] ({columns}) VALUES ({values}); SELECT CAST(SCOPE_IDENTITY() as bigint)";
            _cachedInserts[type] = sql;

            _logger.LogInformation(
                "Generated parameterized SQL INSERT statement for {Type} types into the {Table} table. Query='{Query}'",
                type, tableName, sql);

            return sql;
        }
    }
}