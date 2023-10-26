using System;
using System.Net.Http;

namespace SI.Discord.Webhooks
{
    public interface IWebhookRequest
    {
        public HttpContent CreatePayload();
    }
}