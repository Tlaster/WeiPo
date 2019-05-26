using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiPo.Common
{
    internal static class Singleton<T> where T : new()
    {
        private static readonly ConcurrentDictionary<Type, T> Instances = new ConcurrentDictionary<Type, T>();

        public static T Instance => Instances.GetOrAdd(typeof(T), t => new T());
    }
}
