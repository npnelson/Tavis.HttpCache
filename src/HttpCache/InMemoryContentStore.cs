using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Tavis.HttpCache
{
    internal class ResponseContentCacheEntry
    {
        public ResponseContentCacheEntry(IEnumerable<KeyValuePair<string, IEnumerable<string>>> responseContentHeaders,string responseBody)
        {
            ResponseContentHeaders = responseContentHeaders;
            ResponseBody = responseBody;
        }
        public IEnumerable<KeyValuePair<string,IEnumerable<string>>> ResponseContentHeaders { get; private set; }
        public string ResponseBody { get; private set; }
    }
    public class InMemoryContentStore : IContentStore
    {
        private readonly object syncRoot = new object();
        private readonly Dictionary<CacheKey, CacheEntryContainer> _CacheContainers = new Dictionary<CacheKey, CacheEntryContainer>();
        private readonly Dictionary<Guid, HttpResponseMessage> _responseCache = new Dictionary<Guid, HttpResponseMessage>();
        private readonly Dictionary<Guid, ResponseContentCacheEntry> _responseContentCache = new Dictionary<Guid, ResponseContentCacheEntry>();

        public async Task<IEnumerable<CacheEntry>> GetEntriesAsync(CacheKey cacheKey)
        {
            if (_CacheContainers.ContainsKey(cacheKey)) 
            {
                return _CacheContainers[cacheKey].Entries;
            }
            return null;
        }

        public async Task<HttpResponseMessage> GetResponseAsync(Guid variantId)
        {
            return await CloneResponseAsync(_responseCache[variantId], _responseContentCache[variantId]).ConfigureAwait(false);
        }

        public async Task AddEntryAsync(CacheEntry entry, HttpResponseMessage response)
        {
            CacheEntryContainer cacheEntryContainer = GetOrCreateContainer(entry.Key);
            string responseContent = null;
            if (response.Content != null)
            {                            
                 responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);               
            }
            lock (syncRoot)
            {
                cacheEntryContainer.Entries.Add(entry);
                _responseCache[entry.VariantId] = response;
             
                _responseContentCache[entry.VariantId] = new ResponseContentCacheEntry(response.Content.Headers,responseContent);
            }
        }

        public async Task UpdateEntryAsync(CacheEntry entry, HttpResponseMessage response)
        {

            CacheEntryContainer cacheEntryContainer = GetOrCreateContainer(entry.Key);
            string responseContent = null;
            if (response.Content != null)
            {
                responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            lock (syncRoot)
            {
                var oldentry = cacheEntryContainer.Entries.First(e => e.VariantId == entry.VariantId);
                cacheEntryContainer.Entries.Remove(oldentry);
                cacheEntryContainer.Entries.Add(entry);
                _responseCache[entry.VariantId] = response;
                _responseContentCache[entry.VariantId] = new ResponseContentCacheEntry(response.Content.Headers, responseContent);
            }
        }

        private CacheEntryContainer GetOrCreateContainer(CacheKey key)
        {
            CacheEntryContainer cacheEntryContainer;

            if (!_CacheContainers.ContainsKey(key))
            {
                cacheEntryContainer = new CacheEntryContainer(key);
                lock (syncRoot)
                {
                    _CacheContainers[key] = cacheEntryContainer;
                }
            }
            else
            {
                cacheEntryContainer = _CacheContainers[key];
            }
            return cacheEntryContainer;
        }

        private async Task<HttpResponseMessage> CloneResponseAsync(HttpResponseMessage response, ResponseContentCacheEntry content)
        {
            var newResponse = new HttpResponseMessage(response.StatusCode);
           // MemoryStream ms;

            foreach (var v in response.Headers) newResponse.Headers.TryAddWithoutValidation(v.Key, v.Value);


            if (content != null)
            {
            
                newResponse.Content = new StringContent(content.ResponseBody);
                foreach (var v in content.ResponseContentHeaders) newResponse.Content.Headers.TryAddWithoutValidation(v.Key, v.Value);
            }

            return newResponse;
        }
    }

    public class CacheEntryContainer
    {
        public CacheKey PrimaryCacheKey { get; set; }
        public List<CacheEntry> Entries { get; set; }

        public CacheEntryContainer(CacheKey primaryCacheKey)
        {
            PrimaryCacheKey = primaryCacheKey;
            Entries = new List<CacheEntry>();
        }
    }
}