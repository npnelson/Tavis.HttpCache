using Microsoft.Rest;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tavis.HttpCache;
using Tavis.Test;

namespace Tavis.TestHarness
{
    class Program
    {
        static async Task Main(string[] args)
        {
           
            var httpCache = new Tavis.HttpCache.HttpCache(new InMemoryContentStore());
              var cachingHandler = new HttpCacheHandler(null, httpCache);
            var baseHandler = new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip };
            //var cachingHandler = new TestHandler(null);
            var client = new TestServiceClient(new BasicAuthenticationCredentials(), new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip },cachingHandler);
           
           //  var client = new TestServiceClient(new BasicAuthenticationCredentials(),new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip });
          // var client = new TestServiceClient(new BasicAuthenticationCredentials());
            client.BaseUri = new Uri("http://localhost:52837");
         
           
            Console.ReadLine();
            var response = await client.GetTestOUsAsync();
           
            Console.WriteLine("1st req");
          //  Console.ReadLine();
            var response2 = await client.GetTestOUsAsync();
            // var content2 = await response.Content.ReadAsStringAsync();
            Console.WriteLine("2nd req");
            // Console.ReadLine();
            await Task.Delay(TimeSpan.FromSeconds(30));
            var result3 = await client.GetTestOUsAsync();
            Console.WriteLine("Done");
            Console.ReadLine();
        }
        private static HttpRequestMessage CreateRequest()
        {
            var req = new HttpRequestMessage();
            req.Method = new HttpMethod("GET");
            req.RequestUri = new Uri("http://localhost:52837/api/values");
            return req;

        }
    }

    public class TestHandler : DelegatingHandler
    {
        public TestHandler(HttpMessageHandler innerHandler):base(innerHandler)
        {
            var temp = innerHandler; 
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {            
            var response = await base.SendAsync(request, cancellationToken);
            var responseString = await response.Content.ReadAsStringAsync();
            return response;
        }
    }
}
