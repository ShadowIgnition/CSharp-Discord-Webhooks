using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SI.Discord.Webhooks
{
    public interface IWebhookClient : IDisposable
    {
        Task<HttpResponseMessage> Post(string requestUri, WebhookRequest request);
    }
}