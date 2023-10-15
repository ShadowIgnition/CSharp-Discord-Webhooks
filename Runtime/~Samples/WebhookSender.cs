using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SI.Discord.Webhooks
{
    /// <summary>
    /// Class for sending webhooks
    /// </summary>
    public static class WebhookSender
    {
        /// <summary>
        /// The example URL for the webhook.
        /// </summary>
        public const string EXAMPLEURL = "https://discord.com/api/webhooks/1147121104377368626/_8lkcagoSgw3vR6cNO-kLWKMusFJNNpqM-i8bIlqsKnmh1nL9EEdmgZYJDl6UdpZz1PP";

        /// <summary>
        /// Asynchronously sends a webhook message to the specified URL.
        /// </summary>
        /// <param name="requestURL">The URL to send the webhook to.</param>
        /// <param name="hookObject">The HookObject containing the webhook message data.</param>
        /// <returns>A task representing the HTTP response received from the webhook request.</returns>
        public static async Task SendHookAsync(string requestURL, HookObject hookObject)
        {
            // Check if the requestURL is null or empty
            if (string.IsNullOrWhiteSpace(requestURL))
            {
                throw new ArgumentNullException(nameof(requestURL));
            }

            try
            {
                // Create a new HttpClient for sending the request
                using (HttpClient client = new HttpClient())
                {
                    // Create a form data for multipart content
                    using (MultipartFormDataContent formData = new MultipartFormDataContent("--boundary"))
                    {
                        foreach (var embed in hookObject.Embeds)
                        {
                            // Check if the embed contains an image file
                            if (embed.Image != null && embed.Image.IsFile)
                            {
                                // Add the image file to the form data
                                hookObject.EmbedFile(formData, embed.Image, "image/png", Path.GetFileName(embed.Image.AbsoluteUri));
                            }

                            // Check if the embed contains a thumbnail file
                            if (embed.Thumbnail != null && embed.Thumbnail.IsFile)
                            {
                                // Add the thumbnail file to the form data
                                hookObject.EmbedFile(formData, embed.Thumbnail, "image/png", Path.GetFileName(embed.Thumbnail.AbsoluteUri));
                            }

                            // Check if the embed contains a file
                            if (embed.File != null && embed.File.IsFile)
                            {
                                // Add the file to the form data
                                hookObject.EmbedFile(formData, embed.File, "multipart/mixed", Path.GetFileName(embed.File.AbsoluteUri));
                            }
                        }

                        // Create the JSON object structure with the embedded image
                        JObject json = hookObject.ToJObject();

                        // Convert the JSON object to a string
                        string jsonStr = json.ToString();

                        // Create a StringContent with JSON data
                        StringContent jsonContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                        // Add JSON content to formData
                        formData.Add(jsonContent, "payload_json");

                        // Send the POST request and return the response
                        await client.PostAsync(requestURL, formData);
                    }
                }
            }
            catch (Exception e)
            {
                // Log an error message if an exception occurs
                Console.WriteLine($"Error sending webhook: {e.Message}");
            }
        }
    }
}