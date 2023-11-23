using SI.Discord.Webhooks.Models;
using SI.Discord.Webhooks.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SI.Discord.Webhooks.Services
{
    /// <summary>
    /// Represents a service for sending Discord webhooks.
    /// </summary>
    public class WebhookService : IWebhookService, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the WebhookService class with the provided request URI.
        /// </summary>
        /// <param name="requestURI">The URI to send the webhook request to.</param>
        public WebhookService(Uri requestURI) : this(requestURI.AbsoluteUri, new WebhookClient(new HttpClient())) { }

        /// <summary>
        /// Initializes a new instance of the WebhookService class with the provided request URI and webhook client.
        /// </summary>
        /// <param name="requestURI">The URI to send the webhook request to.</param>
        /// <param name="webhookClient">The client responsible for sending the webhook request.</param>
        public WebhookService(Uri requestURI, IWebhookClient webhookClient) : this(requestURI.AbsoluteUri, webhookClient) { }

        /// <summary>
        /// Initializes a new instance of the WebhookService class with the provided request URI.
        /// </summary>
        /// <param name="requestURI">The URI to send the webhook request to.</param>
        public WebhookService(string requestURI) : this(requestURI, new WebhookClient(new HttpClient())) { }

        /// <summary>
        /// Initializes a new instance of the WebhookService class with the provided request URI and webhook client.
        /// </summary>
        /// <param name="requestURI">The URI to send the webhook request to.</param>
        /// <param name="webhookClient">The client responsible for sending the webhook request.</param>
        public WebhookService(string requestURI, IWebhookClient webhookClient)
        {
            m_RequestURI = requestURI;
            m_WebhookClient = webhookClient;
        }

        /// <summary>
        /// Sends a webhook asynchronously to the specified URI.
        /// </summary>
        /// <param name="requestUri">The URI to send the webhook request to.</param>
        /// <param name="hookObjectToken">The HookObject containing the webhook data.</param>
        /// <returns>The HttpResponseMessage from the webhook request.</returns>
        public async static Task<WebhookResponse> SendWebhookAsync(Uri requestUri, WebhookRequest hookObjectToken) => await SendWebhookAsync(requestUri.AbsoluteUri, hookObjectToken);

        /// <summary>
        /// Sends a webhook asynchronously to the specified URI.
        /// </summary>
        /// <param name="requestUri">The URI to send the webhook request to.</param>
        /// <param name="hookObjectToken">The HookObject containing the webhook data.</param>
        /// <returns>The HttpResponseMessage from the webhook request.</returns>
        public async static Task<WebhookResponse> SendWebhookAsync(string requestUri, WebhookRequest hookObjectToken)
        {
            using (WebhookService webhookService = new(requestUri))
            {
                // Send the webhook asynchronously.
                return await webhookService.SendWebhookAsync(hookObjectToken);
            }
        }

        /// <summary>
        /// Sends a webhook asynchronously with the specified HookObject.
        /// </summary>
        /// <param name="hookObjectToken">The HookObject containing the webhook data.</param>
        /// <returns>The HttpResponseMessage from the webhook request.</returns>
        public async Task<WebhookResponse> SendWebhookAsync(WebhookRequest request)
        {
            bool passedValidation = true;

            List<string> failureReasons = new();
            request.PrimaryToken.Validate();
            foreach (var token in request.SecondaryTokens)
            {
                token.Validate();
            }

            return await m_WebhookClient.Post(m_RequestURI, request);
        }

        /// <summary>
        /// Disposes of the WebhookService and its associated resources.
        /// </summary>
        public void Dispose()
        {
            m_WebhookClient?.Dispose();
        }

        readonly string m_RequestURI;
        readonly IWebhookClient m_WebhookClient;
    }
}
