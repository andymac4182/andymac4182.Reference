using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class ObjectExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this T self)
        {
            yield return self;
        }

        public static Task<T> AsTask<T>(this T self)
        {
            return Task.FromResult(self);
        }

        public static T If<T>(this T obj, bool predicate, Action<T> configureAction)
        {
            if (predicate)
                configureAction(obj);
            return obj;
        }

        public static bool HasAttribute<T>(this object obj) where T : Attribute
        {
            return obj.GetType().GetTypeInfo().GetCustomAttribute<T>() != null;
        }
    }
}