using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TylerSteiner.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static async Task<IList<T>> ToListAsync<T>(this Task<IEnumerable<T>> source)
        {
            return (await source).ToList();
        } 
    }
}