using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebApi.Tests.Common
{
    public static class HttpClientUtilities
    {
        public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string requestUri, object body)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(body),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            return client.PostAsync(requestUri, content);
        }

        public static async Task<TResponse> ReadContent<TResponse>(this HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        public static async Task<TResponse> ReadContentAsAnonymous<TResponse>(this HttpResponseMessage response, TResponse anonymous)
        {
            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeAnonymousType(content, anonymous);
        }
    }
}