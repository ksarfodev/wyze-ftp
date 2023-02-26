
namespace WyzeFtpLibrary
{
    public abstract class BaseHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> ProcessRequest(HttpRequestMessage httpRequestMessage, string uri)
        {
            HttpResponseMessage responseMessage; 

            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri(uri);

                responseMessage = await client.SendAsync(httpRequestMessage);
                responseMessage.EnsureSuccessStatusCode();
            }

            return responseMessage;
        }
    }
}
