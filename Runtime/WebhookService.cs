using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SI.Discord.Webhooks
{
    public class WebhookService : IWebhookService, IDisposable
    {
        public WebhookService(Uri requestURI) : this(requestURI.AbsoluteUri, new WebhookClient(new HttpClient())) { }
        public WebhookService(Uri requestURI, IWebhookClient webhookClient) : this(requestURI.AbsoluteUri, webhookClient) { }
        public WebhookService(string requestURI) : this(requestURI, new WebhookClient(new HttpClient())) { }
        public WebhookService(string requestURI, IWebhookClient webhookClient)
        {
            m_RequestURI = requestURI;
            m_HookObjectValidator = new();
            m_WebhookClient = webhookClient;
        }

        public static void SendWebhook(Uri requestUri, HookObject hookObject) => SendWebhook(requestUri.AbsoluteUri, hookObject);
        public static void SendWebhook(string requestUri, HookObject hookObject)
        {
            _ = SendWebhookAsync(requestUri, hookObject);
        }

        public async static Task<HttpResponseMessage> SendWebhookAsync(Uri requestUri, HookObject hookObject) => await SendWebhookAsync(requestUri.AbsoluteUri, hookObject);
        public async static Task<HttpResponseMessage> SendWebhookAsync(string requestUri, HookObject hookObject)
        {
            using (WebhookService webhookService = new(requestUri))
            {
                // Send the webhook asynchronously.
                return await webhookService.SendWebhookAsync(hookObject);
            }
        }

        public async Task<HttpResponseMessage> SendWebhookAsync(HookObject hookObject)
        {
            bool passedValidation = true;

            List<string> failureReasons = new();

            if (!m_HookObjectValidator.ExceedsEmbedLimit(hookObject))
            {
                passedValidation = false;
                failureReasons.Add($"More than {HookObject.MAX_EMBEDS} Embedded files is not supported on a discord webhook");
            }

            if (!m_HookObjectValidator.HasValidUsername(hookObject, out string reason))
            {
                passedValidation = false;
                failureReasons.Add("Invalid Username: " + reason);
            }

            if (!passedValidation)
            {
                string reasons = string.Join(", ", failureReasons);
                HttpResponseMessage errorResponse = new(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"An error occurred while sending the webhook, reasons: {reasons}")
                };
                return errorResponse;
            }
            WebhookRequest request = new(hookObject);
            return await m_WebhookClient.Post(m_RequestURI, request);
        }

        public void SendWebhook(HookObject hookObject)
        {
            _ = SendWebhookAsync(hookObject);
        }

        public void Dispose()
        {
            m_WebhookClient?.Dispose();
        }

        readonly string m_RequestURI;
        readonly HookObjectValidator m_HookObjectValidator;
        readonly IWebhookClient m_WebhookClient;
    }
}