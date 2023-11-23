using SI.Discord.Webhooks.Models;
using System;
using System.Collections.Generic;
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

        public async Task<HttpResponseMessage> PostSingle(string requestUri, HookPayload payload)
        {
            var a= await  m_HttpClient.PostAsync(requestUri, payload.Content);
            return a;
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
