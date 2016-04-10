using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace TylerSteiner.Services.Data
{
    public interface IDbConnectionFactory
    {
        Task<TResult> UseAsync<TResult>(Func<DbConnection, Task<TResult>> action);
    }
}