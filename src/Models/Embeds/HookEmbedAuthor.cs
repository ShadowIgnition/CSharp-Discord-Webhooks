using Newtonsoft.Json.Linq;
using SI.Discord.Webhooks.Utilities;
using System;

namespace SI.Discord.Webhooks.Models
{
    /// <summary>
    /// Represents an author in an embedded message hook.
    /// </summary>
    public struct HookEmbedAuthor : IConvertibleToJObject
    {
        /// <summary>
        /// Initializes a new instance of the HookEmbedAuthor struct with optional author information.
        /// <para>If all arguments (name, url, and iconURL) are null throws <see cref="ArgumentException"/>.</para>
        /// </summary>
        /// <param name="name">The display name of the embed author (optional).</param>
        /// <param name="url">The URL of the embed author, allowing users to click the author name to visit this link (optional).</param>
        /// <param name="iconURL">The URL of the author's icon, which will be displayed as an icon (optional).</param>
        /// <exception cref="ArgumentException">Thrown when all arguments (name, url, and iconURL) are null.</exception>
        public HookEmbedAuthor(string name, string url, string iconURL)
        {
            Name = name;

            URiUtils.TryParseURI(url, out Uri resultURL);
            URL = resultURL;

            URiUtils.TryParseURI(iconURL, out resultURL);
            Icon_URL = resultURL;
        }

        /// <summary>
        /// Gets the display name of the embed author.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the URL of the embed author, allowing users to click the author name to visit this link.
        /// </summary>
        public Uri URL { get; private set; }

        /// <summary>
        /// Gets the URL of the author's icon, which will be displayed as an icon.
        /// </summary>
        public Uri Icon_URL { get; private set; }

        /// <summary>
        /// Converts the HookEmbedAuthor object to a JObject for serialization.
        /// </summary>
        /// <returns>A JObject containing the author information.</returns>
        public readonly JObject ToJObject()
        {
            JObject root = new();

            if (!string.IsNullOrEmpty(Name))
            {
                root.Add(nameof(Name).ToLowerInvariant(), Name);
            }
            if (URL != null)
            {
                root.Add(nameof(URL).ToLowerInvariant(), URL.AbsoluteUri);
            }
            if (Icon_URL != null)
            {
                root.Add(nameof(Icon_URL).ToLowerInvariant(), Icon_URL.AbsoluteUri);
            }
            return root;
        }
    }
}