using SI.Discord.Webhooks.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace SI.Discord.Webhooks.Services
{
    /// <summary>
    /// Represents an interface for a webhook sending service.
    /// </summary>
    public interface IWebhookService
    {
        /// <summary>
        /// Sends a webhook asynchronously.
        /// </summary>
        /// <param name="hookObject">The HookObject containing webhook data.</param>
        /// <returns>The HttpResponseMessage returned from the webhook request.</returns>
        public Task<HttpResponseMessage> SendWebhookAsync(HookObject hookObject);
    }
}
