// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        public static string Join(this IEnumerable<string> toJoin, string separator) => string.Join(separator, toJoin);
    }
}