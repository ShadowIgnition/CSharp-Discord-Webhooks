using Newtonsoft.Json.Linq;
using System;

namespace SI.Discord.Webhooks
{

    /// <summary>
    /// Represents an author in an embedded message hook.
    /// </summary>
    public struct HookEmbedAuthor : IHookContainer
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
            if (name == null && url == null && iconURL == null)
            {
                throw new ArgumentException("At least one argument must be non-null");
            }

            Name = name;
            URL = URiHelper.EazyURi(url);
            Icon_URL = URiHelper.EazyURi(iconURL);
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
        public JObject ToJObject()
        {
            JObject root = new();

            if (!string.IsNullOrEmpty(Name))
            {
                root.Add(nameof(Name).ToLower(), Name);
            }
            if (URL != null)
            {
                root.Add(nameof(URL).ToLower(), URL.AbsoluteUri);
            }
            if (Icon_URL != null)
            {
                root.Add(nameof(Icon_URL).ToLower(), Icon_URL.AbsoluteUri);
            }
            return root;
        }
    }
}