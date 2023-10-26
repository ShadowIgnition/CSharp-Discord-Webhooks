using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Text;

namespace SI.Discord.Webhooks
{
    public class WebhookRequest : IWebhookRequest
    {
        readonly HookObject m_HookObject;

        public WebhookRequest(HookObject hookObject)
        {
            m_HookObject = hookObject;
        }

        public HttpContent CreatePayload()
        {
            MultipartFormDataContent formData = new();
            foreach (var embed in m_HookObject.Embeds)
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

            // Create the JSON object structure with the embedded image
            JObject json = m_HookObject.ToJObject();

            // Convert the JSON object to a string
            string jsonStr = json.ToString();

            // Create a StringContent with JSON data
            StringContent jsonContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");

            // Add JSON content to formData
            formData.Add(jsonContent, "payload_json");

            return formData;
        }
    }
}