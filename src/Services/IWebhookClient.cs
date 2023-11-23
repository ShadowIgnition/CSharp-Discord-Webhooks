using Newtonsoft.Json;
using SI.Discord.Webhooks.Models;
using System;
using System.Collections.Generic;
using System.Net;
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
        Task<HttpResponseMessage> PostSingle(string requestUri, HookPayload payload);

        Task<HttpResponseMessage> PostSingle(string requestUri, string threadID, HookPayload payload)
        {
            if (string.IsNullOrEmpty(threadID))
            {
                return PostSingle(requestUri, payload);
            }
            return PostSingle(requestUri + string.Format(THREAD_ID, threadID), payload);
        }

        public async Task<WebhookResponse> Post(string requestUri, WebhookRequest request)
        {
            WebhookResponse response = await PostAndReadSingle(requestUri, new( request.CreatePayload()));

            // todo: set thread name?
            if (!response.IsSuccess)
            {
                return response;
            }

            try
            {
                List<Task<HttpResponseMessage>> responseTasks = new();

                foreach (IHookObjectToken subObject in request.SecondaryTokens)
                {
                    responseTasks.Add(PostSingle(requestUri, response.WebhookObject.Id.ToString(), subObject.CreatePayload()));
                }
                HttpResponseMessage[] responses = await Task.WhenAll(responseTasks);
                response.AddMessages(responses);
            }
            catch (Exception ex)
            {
                // If an exception occurs, create and return an error response
                HttpResponseMessage errorResponse = new(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"A client error occurred while sending the webhook: {ex.Message}")
                };
                response.AddMessages(errorResponse);
            }
            return response;
        }

        async Task<WebhookResponse> PostAndReadSingle(string requestUri, HookPayload payload)
        {
            HttpResponseMessage responseMessage = await PostSingle(requestUri, payload);

            // Read the response content
            string responseContent = await responseMessage.Content.ReadAsStringAsync();

            // Deserialize the response to extract the webhook object data (like id)
            WebhookObject webhookObject = JsonConvert.DeserializeObject<WebhookObject>(responseContent);

            return new WebhookResponse(webhookObject, responseMessage);
        }

        const string THREAD_ID = "?thread_id={threadId}";
    }
}
