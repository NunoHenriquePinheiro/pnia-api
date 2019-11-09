using PniaApi.Models.ExternalRequests;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PniaApi.ExternalRequests
{
    /// <summary>Class of methods to invoke services.</summary>
    public class ServiceInvoker
    {
        private const string _urlSeparator = "/";
        private string _url { get; set; }

        public ServiceInvoker(string url)
        {
            _url = url;
        }

        /// <summary>Performs a GET request.</summary>
        /// <param name="inputParameter">The input parameters, in a string format.</param>
        /// <returns>Result of the service call - its content and if it was successful.</returns>
        public async Task<ServiceResult> GetRequest(string inputParameters)
        {
            var requestUrl = _url + _urlSeparator + inputParameters;
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(contentType);
                var response = await client.SendAsync(request);

                return new ServiceResult()
                {
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    ContentResult = response.Content?.ReadAsStringAsync().Result
                };
            }
        }
    }
}
