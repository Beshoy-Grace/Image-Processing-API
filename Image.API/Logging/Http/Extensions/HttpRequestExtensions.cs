using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Image.API.Logging.Http.Extensions
{
    public static class HttpRequestExtensions
    {
        public static async Task<string> ReadBodyAsync(this HttpRequest request)
        {
            if (request.Method.EqualsIgnoreCaseAny("POST", "PUT"))
            {
                var returnValue = string.Empty;
                //use the leaveOpen parameter as true so further reading and processing of the request body can be done down the pipeline
                using (var stream = new StreamReader(request.Body, Encoding.UTF8, true, 1024, leaveOpen: true))
                {
                    returnValue = await stream.ReadToEndAsync();
                }
                //reset position to ensure other readers have a clear view of the stream 
                //request.Body.Position = 0;
                return returnValue;

                //try
                //{
                //    request.EnableBuffering();

                //    using (StreamReader reader = new StreamReader(request.Body))
                //    {
                //        string reqBody = JsonConvert.SerializeObject(TaskHelper.RunSync(() => reader.ReadToEndAsync()));
                //        return reqBody;
                //    }

                //}
                //finally
                //{
                //    request.Body = initialBody;
                //}
            }

            return string.Empty;
        }

        public static Uri GetAbsoluteUri(this HttpRequest request)
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();
            return uriBuilder.Uri;
        }
    }
}