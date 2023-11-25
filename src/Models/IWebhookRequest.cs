using System.Net.Http;

namespace SI.Discord.Webhooks.Models
{
    /// <summary>
    /// Represents an interface for a webhook request.
    /// </summary>
    public interface IWebhookRequest
    {
        /// <summary>
        /// Creates the payload content for the webhook request.
        /// </summary>
        /// <returns>The HTTP content representing the payload.</returns>
        HttpContent CreatePayload();
    }
}
