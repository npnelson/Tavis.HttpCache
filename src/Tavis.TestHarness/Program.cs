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
            Console.ReadLine();
            var result = await client.GetStringAsync("api/values");
            Console.WriteLine("1st req");
            Console.ReadLine();
            var result2 = await client.GetStringAsync("api/values");
            Console.WriteLine("2nd req");
            Console.ReadLine();
            await Task.Delay(TimeSpan.FromSeconds(30));
            var result3 = await client.GetStringAsync("api/values");
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
