using Newtonsoft.Json;
using SI.Discord.Webhooks.Models;
using SI.Discord.Webhooks.Services;
using SI.Discord.Webhooks.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SI.Discord.Webhooks.Examples
{
    /// <summary>
    /// A static class for demonstrating webhook functionality.
    /// </summary>
    public static class Example
    {
        /// <summary>
        /// Nested static class containing methods related to forum interactions.
        /// </summary>
        public static class Forums
        {
            /// <summary>
            /// Sends an example message to a forum thread using a webhook asynchronously.
            /// </summary>
            /// <param name="webhookURL">The URL of the webhook.</param>
            /// <param name="threadID">The ID of the forum thread.</param>
            /// <param name="hookObject">Optional hook object to send.</param>
            /// <returns>A task returning the result of the webhook operation.</returns>
            public static async Task<Result<string>> SendToForum(string webhookURL, string threadID, HookObject? hookObject = null, string localThumbnailPath = null)
            {
                if (hookObject.Equals(default))
                {
                    Objects.CreatePrimaryHookEmbed(Guid.NewGuid().ToString(), localThumbnailPath, out HookEmbed hookEmbed);
                    Result<string> result = Objects.TryCreatePrimaryHookObject(null, new[] { hookEmbed }, out HookObject hook);
                    hookObject = hook;
                    if (result.Failed)
                    {
                        return result;
                    }
                }

                // Create service
                using (WebhookService webhookService = new(webhookURL))
                {
                    HttpResponseMessage responseMessage = await webhookService.SendWebhookAsync(hookObject.Value, threadID);

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        return responseMessage.StatusCode.ToString();
                    }
                }
                return Result<string>.Success;
            }

            /// <summary>
            /// Sends an example message to a forum thread using a webhook asynchronously.
            /// </summary>
            /// <param name="webhookURL">The URL of the webhook.</param>
            /// <param name="threadName">The name of the forum thread.</param>
            /// <param name="id">Optional ID for the thread.</param>
            /// <returns>A task returning the result of the webhook operation.</returns>
            public static async Task<Result<string>> SendToForum(string webhookURL, string threadName, string id = null, string localThumbnailPath = null)
            {
                Objects.CreatePrimaryHookEmbed(id, localThumbnailPath, out HookEmbed hookEmbed);
                Result<string> result = Objects.TryCreatePrimaryHookObject(threadName, new[] { hookEmbed }, out HookObject hookObject);
                if (result.Failed)
                {
                    return result;
                }

                // Create service
                using (WebhookService webhookService = new(webhookURL))
                {
                    HttpResponseMessage responseMessage = await webhookService.SendWebhookAsync(hookObject);

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        return responseMessage.StatusCode.ToString();
                    }

                    string responseContent = await responseMessage.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(responseContent))
                    {
                        return HttpStatusCode.NoContent.ToString();
                    }

                    // Deserialize the response to extract the webhook object data (like id)
                    WebhookObject webhookObject = JsonConvert.DeserializeObject<WebhookObject>(responseContent);

                    Objects.CreateSecondaryHookEmbed(id, out HookEmbed embed);

                    result = Objects.TryCreateSecondaryHookObject(null, new[] { embed }, out hookObject);
                    if (result.Failed)
                    {
                        return result;
                    }

                    return await SendToForum(webhookURL, webhookObject.Id.ToString(), hookObject);
                }
            }
        }

        /// <summary>
        /// Nested static class containing methods related to channel interactions.
        /// </summary>
        public static class Channels
        {
            /// <summary>
            /// Sends an example message to a channel using a webhook asynchronously.
            /// </summary>
            /// <param name="webhookURL">The URL of the webhook.</param>
            /// <returns>A task returning the result of the webhook operation.</returns>
            public static async Task<Result<string>> SendToChannel(string webhookURL, string localThumbnailPath  = null)
            {
                Objects.CreatePrimaryHookEmbed(null, localThumbnailPath, out HookEmbed hookEmbed);
                Result<string> result = Objects.TryCreatePrimaryHookObject(null, new[] { hookEmbed }, out HookObject hookObject);
                if (result.Failed)
                {
                    return result;
                }

                Objects.CreateSecondaryHookEmbed(null, out HookEmbed embed);
                Objects.TryCreateSecondaryHookObject(null, new[] { embed }, out HookObject secondaryHookObject);
                if (result.Failed)
                {
                    return result;
                }

                // Create service
                using (WebhookService webhookService = new(webhookURL))
                {
                    List<Task> tasks = new()
                    {
                        webhookService.SendWebhookAsync(hookObject),
                        webhookService.SendWebhookAsync(secondaryHookObject)
                    };

                    await Task.WhenAll(tasks);
                    return Result<string>.Success;
                }
            }
        }

        /// <summary>
        /// Helper methods to create webhook objects and embeds.
        /// </summary>
        public static class Objects
        {
            /// <summary>
            /// Creates a primary hook object with the specified thread name and embeds.
            /// </summary>
            /// <param name="threadName">The name of the forum thread.</param>
            /// <param name="embeds">The array of embeds to include.</param>
            /// <param name="hookObject">The resulting hook object.</param>
            /// <returns>The result of the operation.</returns>
            public static Result<string> TryCreatePrimaryHookObject(string threadName, HookEmbed[] embeds, out HookObject hookObject)
            {
                HookObjectBuilder hookObjectBuilder = new();
                hookObjectBuilder.SetForumThreadName(threadName);

                foreach (HookEmbed embed in embeds)
                {
                    Result<string> result = hookObjectBuilder.TryAddEmbed(embed);
                    if (result.Failed)
                    {
                        hookObject = default;
                        return result;
                    }
                }

                hookObject = hookObjectBuilder.Build();
                return Result<string>.Success;
            }

            /// <summary>
            /// Creates a primary hook embed with the specified ID and configuration.
            /// </summary>
            /// <param name="id">The ID for the embed.</param>
            /// <param name="embed">The resulting hook embed.</param>
            /// <returns>The result of the operation.</returns>
            public static Result<string> CreatePrimaryHookEmbed(string id, string thumbnailLocalPath, out HookEmbed embed)
            {
                // Create an embed for the message.
                HookEmbedBuilder builder = new HookEmbedBuilder()
                .SetDescription("🚀 Let the automation begin! 🤖🌟")
                .SetTimestamp(DateTime.UtcNow)
                .SetTitle("🎉 Webhooks are here!")
                .SetColor(System.Drawing.Color.IndianRed)
                .SetText("Part 1/2 - " + id)
                .AddField(new HookEmbedField("Field", "A webhook is a mechanism that allows one system to send real-time data to another system as soon as an event occurs, enabling seamless communication and automated processes between different applications or platforms.", false))
                .SetAuthor(new HookEmbedAuthor("New Feedback!", null, AVATAR_URL));

                if (!string.IsNullOrWhiteSpace(thumbnailLocalPath))
                {
                    Result<string> result = builder.TrySetThumbnailURL(thumbnailLocalPath);
                    if (result.Failed)
                    {
                        embed = default;
                        return result;
                    }
                }
                embed = builder.Build();
                return Result<string>.Success;
            }

            /// <summary>
            /// Creates a secondary hook object with the specified thread name and embeds.
            /// </summary>
            /// <param name="threadName">The name of the forum thread.</param>
            /// <param name="embeds">The array of embeds to include.</param>
            /// <param name="hookObject">The resulting hook object.</param>
            /// <returns>The result of the operation.</returns>
            public static Result<string> TryCreateSecondaryHookObject(string threadName, HookEmbed[] embeds, out HookObject hookObject)
            {
                HookObjectBuilder hookObjectBuilder = new();
                hookObjectBuilder.SetForumThreadName(threadName);

                foreach (HookEmbed embed in embeds)
                {
                    Result<string> result = hookObjectBuilder.TryAddEmbed(embed);
                    if (result.Failed)
                    {
                        hookObject = default;
                        return result;
                    }
                }

                hookObject = hookObjectBuilder.Build();
                return Result<string>.Success;
            }

            /// <summary>
            /// Creates a secondary hook embed with the specified ID and configuration.
            /// </summary>
            /// <param name="id">The ID for the embed.</param>
            /// <param name="embed">The resulting hook embed.</param>
            /// <returns>The result of the operation.</returns>
            public static Result<string> CreateSecondaryHookEmbed(string id, out HookEmbed embed)
            {
                string tempFilePath = Path.GetTempFileName();

                // Create an embed for the message.
                HookEmbedBuilder builder = new HookEmbedBuilder()
                    .SetText("Part 2/2 - " + id);

                Result<string> result = builder.TrySetFileURL(tempFilePath);
                if (result.Failed)
                {
                    embed = default;
                    return result;
                }

                embed = builder.Build();
                return Result<string>.Success;
            }
        }

        /// <summary>
        /// The URL for the avatar.
        /// </summary>
        const string AVATAR_URL = "https://static.wikia.nocookie.net/discord/images/0/0d/Clyde_%28sticker%29.svg/revision/latest/scale-to-width-down/180?cb=20211126184816";
    }
}