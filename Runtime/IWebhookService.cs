using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SI.Discord.Webhooks
{
    public interface IWebhookService
    {
        public Task<HttpResponseMessage> SendWebhookAsync(HookObject hookObject);
    }
}