using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SI.Discord.Webhooks
{
    public class WebhookClient : IWebhookClient
    {
        public WebhookClient() : this(new HttpClient()) { }
        public WebhookClient(HttpClient httpClient)
        {
            m_HttpClient = httpClient;
        }

        public async Task<HttpResponseMessage> Post(string requestUri, WebhookRequest request)
        {
            try
            {
                using (HttpContent payload = request.CreatePayload())
                {
                    return await m_HttpClient.PostAsync(requestUri, payload);
                }
            }
            catch (Exception ex)
            {
                // Create and return an error response
                HttpResponseMessage errorResponse = new(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"An error occurred while sending the webhook: {ex.Message}")
                };
                return errorResponse;
            }
        }

        public void Dispose()
        {
            m_HttpClient?.Dispose();
        }

        readonly HttpClient m_HttpClient;
    }
}