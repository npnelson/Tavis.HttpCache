using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace HttpCacheTests
{
    public class CacheableResourceController : Controller
    {
        public HttpResponseMessage Head()
        {
            var response = new HttpResponseMessage();
            response.Headers.Add("CacheableResource","testheader");
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
            {
                MaxAge = new TimeSpan(0, 0, 0, 5)
            };
            response.Content = new StringContent("");
            return response;
        }

        
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("This is cached content")
            };
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() {MaxAge = new TimeSpan(0,0,0,5)};

            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
            {
                MaxAge = new TimeSpan(0, 0, 0, 5)
            };



            return response;
        }


        public HttpResponseMessage Get(string value)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("This is cached content with the value " + value)
            };
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { MaxAge = new TimeSpan(0, 0, 0, 5) };
            return response;
        }
        
    }

    public class PointAController : Controller
    {
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(Url.Link("Default", new {controller = "PointB"}));
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { MaxAge = new TimeSpan(0, 0, 0, 5) };
            return response;
        }
    }

    public class PointBController : Controller
    {
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent("Final Destination");
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { NoCache = true };
            return response;
        }
    }

    public class VaryingCacheableResourceController : Controller
    {
        public HttpResponseMessage Get()
        {           
            StringContent stringContent;          
            if (Request.GetTypedHeaders().AcceptLanguage.Contains(new StringWithQualityHeaderValue("fr")))
            {
                stringContent = new StringContent("Ce donnée est caché");
            }
            else
            {
                stringContent = new StringContent("This is cached content");
            }
             

            var response = new HttpResponseMessage()
            {
                Content = stringContent
            };
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { MaxAge = new TimeSpan(0, 0, 0,60) };
            response.Headers.Vary.Add("accept-language");
            return response;
        }


        public HttpResponseMessage Get(string value)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("This is cached content with the value " + value)
            };
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { MaxAge = new TimeSpan(0, 0, 0, 5) };
            return response;
        }

    }


    public class VaryingCompressedContentController : Controller
    {
        public HttpResponseMessage Get()
        {
            
            HttpContent content = new StringContent("Hello world");

            var response = new HttpResponseMessage();

            if (Request.GetTypedHeaders().AcceptEncoding.Contains(new StringWithQualityHeaderValue("gzip")))
            {
                response.Headers.Vary.Add("Accept-Encoding");
                content = new CompressedContent(content, "gzip");
            }
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { MaxAge = new TimeSpan(0, 0, 0, 60) };

            response.Content = content;
            return response;
        }

    }

    public class VaryStarController : Controller
    {
        public HttpResponseMessage Get()
        {
            HttpContent content = new StringContent("Hello world");

            var response = new HttpResponseMessage();
            response.Headers.Vary.Add("*");
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { MaxAge = new TimeSpan(0, 0, 0, 60) };

            response.Content = content;
            return response;
        }

    }

    [Route("[controller]")]
    public class ResourceWithEtagController : Controller
    {
        [ResponseCache(Duration =300)]
        public HttpResponseMessage Get()
        {
            var etag = new EntityTagHeaderValue("\"XYZPQR\"");
            var etagSystem = new System.Net.Http.Headers.EntityTagHeaderValue("\"XYZPQR\"");

            if (Request.GetTypedHeaders().IfNoneMatch !=null && Request.GetTypedHeaders().IfNoneMatch.Count > 0)
            {
              //  return new HttpResponseMessage(HttpStatusCode.NotFound);

                if (Request.GetTypedHeaders().IfNoneMatch.Contains(etag))
                {
                    var notModifiedresponse = new HttpResponseMessage(HttpStatusCode.NotModified);
                    notModifiedresponse.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { MaxAge = new TimeSpan(0, 0, 0, 5)};
                    notModifiedresponse.Headers.ETag = etagSystem;
                    return notModifiedresponse;
                }
            }
            var content = new StringContent("This is cached content with an etag");
            var response = new HttpResponseMessage()
            {
                Content = content
            };
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { MaxAge = new TimeSpan(0, 0, 0, 5)};
            
            response.Headers.ETag = etagSystem;
            return response;

        }
    }

    public class CacheablePostResponseController : Controller
    {
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("From the server")
            };
            return response;
        }
        
        public async Task<HttpResponseMessage> Post()
        {
            var reader = new StreamReader(Request.Body);
            var requestBodyString = await reader.ReadToEndAsync();
            var response = new HttpResponseMessage()
            {
                Content = new StringContent("Post Response : " + requestBodyString)
            };
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { MaxAge = new TimeSpan(0, 0, 0, 5) };
            
            return response;

        }
    }

    public class CompressedContent : HttpContent
    {
        private readonly HttpContent originalContent;
        private readonly string encodingType;

        public CompressedContent(HttpContent content, string encodingType)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (encodingType == null)
            {
                throw new ArgumentNullException("encodingType");
            }

            originalContent = content;
            this.encodingType = encodingType.ToLowerInvariant();

            if (this.encodingType != "gzip" && this.encodingType != "deflate")
            {
                throw new InvalidOperationException(string.Format("Encoding '{0}' is not supported. Only supports gzip or deflate encoding.", this.encodingType));
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in originalContent.Headers)
            {
                Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            Headers.ContentEncoding.Add(encodingType);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;

            return false;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Stream compressedStream = null;

            if (encodingType == "gzip")
            {
                compressedStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen: true);
            }
            else if (encodingType == "deflate")
            {
                compressedStream = new DeflateStream(stream, CompressionMode.Compress, leaveOpen: true);
            }

            return originalContent.CopyToAsync(compressedStream).ContinueWith(tsk =>
            {
                if (compressedStream != null)
                {
                    compressedStream.Dispose();
                }
            });
        }
    }
}
