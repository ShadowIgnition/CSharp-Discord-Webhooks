using SI.Discord.Webhooks.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SI.Discord.Webhooks.Services
{
    /// <summary>
    /// This interface defines the contract for a webhook client.
    /// </summary>
    public interface IWebhookClient : IDisposable
    {
        /// <summary>
        /// Sends a webhook request to the specified URI.
        /// </summary>
        /// <param name="requestUri">The URI to send the webhook request to.</param>
        /// <param name="request">The WebhookRequest object containing the payload and other details.</param>
        /// <returns>A Task representing the asynchronous operation, which returns an HttpResponseMessage.</returns>
        Task<HttpResponseMessage> Post(string requestUri, WebhookRequest request);
    }
}
