using System;
using System.Threading.Tasks;

namespace SI.Discord.Webhooks
{

    /// <summary>
    /// A static class for demonstrating webhook functionality.
    /// </summary>
    public static class WebhookExample
    {
        /// <summary>
        /// Sends an example message using a webhook asynchronously.
        /// </summary>
        public static async Task SendExample(string webhookURL)
        {
            // Create an embed for the message.
            HookEmbedBuilder embed = new HookEmbedBuilder()
                .SetDescription("🚀 Let the automation begin! 🤖🌟")
                .SetTimestamp(DateTime.UtcNow)
                .SetTitle("🎉 Webhooks are here!")
                .SetColor(System.Drawing.Color.IndianRed)
                .AddField(new HookEmbedField("Field", "A webhook is a mechanism that allows one system to send real-time data to another system as soon as an event occurs, enabling seamless communication and automated processes between different applications or platforms.", false))
                .SetThumbnailURL(THUMBNAIL_URL)
                .SetAuthor(new HookEmbedAuthor("Discord Webhook!", "https://shadowignition.github.io", AVATAR_URL));

            // Build a HookObject with message details.
            HookObjectBuilder hookObjectBuilder = new HookObjectBuilder()
                .SetUsername("Bonnie")
                .SetContent("**This is a content message sent via a webhook!**")
                .SetAvatarUrl(AVATAR_URL)
                .AddEmbed(embed.Build());

            // Build the HookObject.
            HookObject hookObject = hookObjectBuilder.Build();

            // Send the webhook asynchronously.
            await WebhookSender.SendHookAsync(webhookURL, hookObject);
        }

        /// <summary>
        /// The URL for the avatar.
        /// </summary>
        const string AVATAR_URL = "https://static.wikia.nocookie.net/discord/images/0/0d/Clyde_%28sticker%29.svg/revision/latest/scale-to-width-down/180?cb=20211126184816";

        /// <summary>
        /// The URL for the thumbnail.
        /// </summary>
        const string THUMBNAIL_URL = "https://assets-global.website-files.com/5f9072399b2640f14d6a2bf4/643d9e196f9a672e57e79b3f_Community%20Onboarding_Blog%20Header_blog%20header-p-500.jpg";

        /// <summary>
        /// Placeholder for the image path.
        /// </summary>
        const string IMAGE_PATH = "PATH";

        /// <summary>
        /// Placeholder for the file path.
        /// </summary>
        const string FILE_PATH = "PATH";
    }
}