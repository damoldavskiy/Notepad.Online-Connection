using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CognitiveServices
{
    static class Rest
    {
        public static async Task<string> PostAsync(string url, string type, Dictionary<string, string> headers, byte[] data)
        {
            var request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = type;
            foreach (var pair in headers)
                request.Headers.Add(pair.Key, pair.Value);

            using (var stream = await request.GetRequestStreamAsync())
            {
                stream.Write(data, 0, data.Length);
            }

            using (var response = await request.GetResponseAsync())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
