using System.Collections.Generic;

namespace TylerSteiner.Models
{
    public class ImdbEqualityComparer<T> : IEqualityComparer<T> where T : IImdbEntity
    {
        public bool Equals(T x, T y)
        {
            if (x == null && y == null) return true;
            if (x == null | y == null)return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(T obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}