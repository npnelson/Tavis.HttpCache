using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tavis.HttpCache;

namespace Tavis.TestHarness
{
    class Program
    {
        static async Task Main(string[] args)
        {
           
            var httpCache = new Tavis.HttpCache.HttpCache(new InMemoryContentStore());
            var cachingHandler = new HttpCacheHandler(new HttpClientHandler(), httpCache);
            var client = new HttpClient(cachingHandler);
            client.BaseAddress = new Uri("http://localhost:52837");
            var result = await client.GetStringAsync("api/values");
            var result2 = await client.GetStringAsync("api/values");
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
