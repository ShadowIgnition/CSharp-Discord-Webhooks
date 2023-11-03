using SI.Discord.Webhooks.Models;
using SI.Discord.Webhooks.Services;
using SI.Discord.Webhooks.Utilities;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace SI.Discord.Webhooks.Examples
{
    /// <summary>
    /// A static class for demonstrating webhook functionality.
    /// </summary>
    public static class Example
    {
        /// <summary>
        /// Sends an example message using a webhook asynchronously.
        /// </summary>
        public static async Task SendExample(string webhookURL)
        {
            BuildHookEmbed(out HookEmbedBuilder embed);
            BuildHookObject(embed, out HookObject hookObject);

            // Create service
            using (WebhookService webhookService = new(webhookURL))
            {
                HttpResponseMessage responseMessage = await webhookService.SendWebhookAsync(hookObject);
                Debug.Log(responseMessage.IsSuccessStatusCode);
            }
        }

        static HookEmbedBuilder BuildHookEmbed(out HookEmbedBuilder embed)
        {
            string tempFilePath = Path.GetTempFileName();

            // Create an embed for the message.
            embed = new HookEmbedBuilder()
                .SetDescription("🚀 Let the automation begin! 🤖🌟")
                .SetTimestamp(DateTime.UtcNow)
                .SetTitle("🎉 Webhooks are here!")
                .SetColor(Color.red)
                .AddField(new HookEmbedField("Field", "A webhook is a mechanism that allows one system to send real-time data to another system as soon as an event occurs, enabling seamless communication and automated processes between different applications or platforms.", false))
                .SetAuthor(new HookEmbedAuthor("Discord Webhook!", "https://github.com/ShadowIgnition/CSharp-Discord-Webhooks", AVATAR_URL));

            Result<string> result = embed.TrySetThumbnailURL(THUMBNAIL_URL);
            if (result.Failed)
            {
                Debug.LogError(result.Message);
            }

            result = embed.TrySetFileURL(tempFilePath);
            if (result.Failed)
            {
                Debug.LogError(result.Message);
            }

            return embed;
        }

        static void BuildHookObject(HookEmbedBuilder embed, out HookObject hookObject)
        {
            HookObjectBuilder hookObjectBuilder = new();

            Result<string> result = hookObjectBuilder.TryAddEmbed(embed.Build());
            if (result.Failed)
            {
                Debug.LogError(result.Message);
            }

            hookObject = hookObjectBuilder.Build();
        }

        /// <summary>
        /// The URL for the avatar.
        /// </summary>
        const string AVATAR_URL = "https://static.wikia.nocookie.net/discord/images/0/0d/Clyde_%28sticker%29.svg/revision/latest/scale-to-width-down/180?cb=20211126184816";

        /// <summary>
        /// The URL for the thumbnail.
        /// </summary>
        const string THUMBNAIL_URL = "https://assets-global.website-files.com/5f9072399b2640f14d6a2bf4/643d9e196f9a672e57e79b3f_Community%20Onboarding_Blog%20Header_blog%20header-p-500.jpg";
    }
}