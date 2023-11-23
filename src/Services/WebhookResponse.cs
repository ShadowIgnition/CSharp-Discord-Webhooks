using SI.Discord.Webhooks.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SI.Discord.Webhooks.Services
{
    public class WebhookResponse
    {
        public WebhookObject WebhookObject { get; }
        public List<HttpResponseMessage> ResponseMessages { get; }
        public HttpResponseMessage RecentMessage { get { return ResponseMessages[ResponseMessages.Count - 1]; } }
        public bool IsSuccess { get { return RecentMessage.IsSuccessStatusCode; } }
        public WebhookResponse(params HttpResponseMessage[] responseMessages)
        {
            WebhookObject = null;
            ResponseMessages = new(responseMessages);
        }

        public WebhookResponse(WebhookObject webhookObject, params HttpResponseMessage[] responseMessages)
        {
            WebhookObject = webhookObject;
            ResponseMessages = new(responseMessages);
        }

        public void AddMessages(params HttpResponseMessage[] httpResponseMessage)
        {
            ResponseMessages.AddRange(httpResponseMessage);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();
            for (int i = 0; i < ResponseMessages.Count; i++)
            {
                stringBuilder.Append(ResponseMessages[i].ToString());
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }
    }
}
