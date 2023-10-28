using SI.Discord.Webhooks.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SI.Discord.Webhooks.Services
{
    /// <summary>
    /// Represents a client for sending webhooks.
    /// </summary>
    public class WebhookClient : IWebhookClient
    {
        /// <summary>
        /// Initializes a new instance of the WebhookClient class using the default HttpClient.
        /// </summary>
        public WebhookClient() : this(new HttpClient()) { }

        /// <summary>
        /// Initializes a new instance of the WebhookClient class using a custom HttpClient.
        /// </summary>
        /// <param name="httpClient">The HttpClient to be used for sending webhooks.</param>
        public WebhookClient(HttpClient httpClient)
        {
            m_HttpClient = httpClient;
        }

        /// <summary>
        /// Sends a webhook request to the specified URI.
        /// </summary>
        /// <param name="requestUri">The URI to send the webhook request to.</param>
        /// <param name="request">The WebhookRequest object containing the payload and other details.</param>
        /// <returns>A Task representing the asynchronous operation, which returns an HttpResponseMessage.</returns>
        public async Task<HttpResponseMessage> Post(string requestUri, WebhookRequest request)
        {
            try
            {
                // Create an HttpContent payload from the WebhookRequest
                using (HttpContent payload = request.CreatePayload())
                {
                    // Send an asynchronous POST request
                    return await m_HttpClient.PostAsync(requestUri, payload);
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, create and return an error response
                HttpResponseMessage errorResponse = new(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"An error occurred while sending the webhook: {ex.Message}")
                };
                return errorResponse;
            }
        }

        /// <summary>
        /// Disposes of the HttpClient used by the WebhookClient.
        /// </summary>
        public void Dispose()
        {
            m_HttpClient?.Dispose();
        }

        readonly HttpClient m_HttpClient; // The HttpClient used for sending webhooks.
    }
}
