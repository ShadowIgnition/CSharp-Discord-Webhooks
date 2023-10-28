using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Text;

namespace SI.Discord.Webhooks.Models
{
    /// <summary>
    /// Represents a class for creating a webhook request payload.
    /// </summary>
    /// <remarks>
    /// This class is responsible for generating the payload that will be sent in a webhook request.
    /// It processes the provided HookObject to extract embeds and files, converts them to the required format,
    /// and creates a multipart form data content that includes both JSON and file data.
    /// </remarks>
    public class WebhookRequest : IWebhookRequest
    {
        /// <summary>
        /// Initializes a new instance of the WebhookRequest class with a specified HookObject.
        /// </summary>
        /// <param name="hookObject">The HookObject containing information for the webhook.</param>
        public WebhookRequest(HookObject hookObject)
        {
            m_HookObject = hookObject;
        }

        /// <summary>
        /// Creates the payload for the webhook request.
        /// </summary>
        /// <returns>A <see cref="HttpContent"/> object representing the payload.</returns>
        public HttpContent CreatePayload()
        {
            // Initialize a new MultipartFormDataContent
            MultipartFormDataContent formData = new();

            // Iterate through the embeds in the HookObject
            foreach (HookEmbed embed in m_HookObject.Embeds)
            {
                // Check if the embed contains an image file
                if (embed.Image != null && embed.Image.IsFile)
                {
                    // Add the image file to the form data
                    m_HookObject.EmbedFile(formData, embed.Image, "image/png", Path.GetFileName(embed.Image.AbsoluteUri));
                }

                // Check if the embed contains a thumbnail file
                if (embed.Thumbnail != null && embed.Thumbnail.IsFile)
                {
                    // Add the thumbnail file to the form data
                    m_HookObject.EmbedFile(formData, embed.Thumbnail, "image/png", Path.GetFileName(embed.Thumbnail.AbsoluteUri));
                }

                // Check if the embed contains a file
                if (embed.File != null && embed.File.IsFile)
                {
                    // Add the file to the form data
                    m_HookObject.EmbedFile(formData, embed.File, "multipart/mixed", Path.GetFileName(embed.File.AbsoluteUri));
                }
            }

            // Create a JSON object structure from the HookObject
            JObject json = m_HookObject.ToJObject();

            // Convert the JSON object to a string
            string jsonStr = json.ToString();

            // Create a StringContent with JSON data
            StringContent jsonContent = new(jsonStr, Encoding.UTF8, "application/json");

            // Add JSON content to formData
            formData.Add(jsonContent, "payload_json");

            // Return the final payload
            return formData;
        }

        readonly HookObject m_HookObject;
    }
}
