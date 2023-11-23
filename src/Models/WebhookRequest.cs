using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using UnityEngine.UI;

namespace SI.Discord.Webhooks.Models
{
    public class HookPayload
    {
        public HttpContent Content;

        public HookPayload(HttpContent content)
        {
            Content = content;
        }
    }

    /// <summary>
    /// Represents a class for creating a webhook request payload.
    /// </summary>
    /// <remarks>
    /// This class is responsible for generating the payload that will be sent in a webhook request.
    /// It processes the provided HookObject to extract embeds and files, converts them to the required format,
    /// and creates a multipart form data content that includes both JSON and file data.
    /// </remarks>
    public record WebhookRequest
    {
        public IHookObjectToken PrimaryToken { get; }
        public IHookObjectToken[] SecondaryTokens { get; }

        /// <summary>
        /// Initializes a new instance of the WebhookRequest class with a specified HookObject.
        /// </summary>
        /// <param name="hookObject">The HookObject containing information for the webhook.</param>
        public WebhookRequest(IHookObjectToken hookObject, params IHookObjectToken[] subHookObjects)
        {
            PrimaryToken = hookObject;
            SecondaryTokens = subHookObjects;
        }

        /// <summary>
        /// Creates the payload for the webhook request.
        /// </summary>
        /// <returns>A <see cref="HttpContent"/> object representing the payload.</returns>
        public HttpContent CreatePayload()
        {
            // Initialize a new MultipartFormDataContent
            MultipartFormDataContent formData = new();
            if (PrimaryToken is HookObject m_HookObject)
            {
                // Create a JSON object structure from the HookObject
                JObject json = m_HookObject.ToJObject();

                // Convert the JSON object to a string
                string jsonStr = json.ToString();

                // Create a StringContent with JSON data
                StringContent jsonContent = new(jsonStr, Encoding.UTF8, "application/json");

                // Add JSON content to formData
                formData.Add(jsonContent, "payload_json");
            }



            // Return the final payload
            return formData;
        }
    }
}
