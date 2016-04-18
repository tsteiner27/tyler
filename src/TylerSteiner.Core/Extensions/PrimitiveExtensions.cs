using System;
using System.Globalization;

namespace TylerSteiner.Core.Extensions
{
    public static class PrimitiveExtensions
    {
        public static string Truncate(this double number, int digits)
        {
            var str = number.ToString(CultureInfo.InvariantCulture);
            var dot = str.IndexOf(".", StringComparison.InvariantCulture);
            return str.Substring(0, dot + digits + 1);
        }
    }
}