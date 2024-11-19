using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Classes
{
    public class DataCache
    {
        private readonly Dictionary<string, (DateTime, string)> _cache = new();
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        public string GetOrAdd(string key, Func<string> loadFunction)
        {
            if (_cache.TryGetValue(key, out var cacheEntry) &&
                DateTime.Now - cacheEntry.Item1 < _cacheDuration)
            {
                Console.WriteLine("get"+ key);
                return cacheEntry.Item2;
            }

            string data = loadFunction();
            _cache[key] = (DateTime.Now, data);
            Console.WriteLine("add"+ key);
            return data;
        }

        public void Invalidate(string key)
        {
            if (_cache.ContainsKey(key))
            {
                Console.WriteLine("remove"+key);
                _cache.Remove(key);
            }
        }
    }
}
