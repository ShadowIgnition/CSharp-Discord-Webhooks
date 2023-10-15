using System;
using System.Collections.Generic;
using System.Linq;

namespace SI.Discord.Webhooks
{

    /// <summary>
    /// Represents a builder for creating a HookObject used in a Discord webhook.
    /// </summary>
    public class HookObjectBuilder
    {
        /// <summary>
        /// Gets a value indicating whether the number of embedded files has reached the maximum limit.
        /// </summary>
        /// <remarks>
        /// This property returns true if the number of embedded files equals the maximum allowed limit.
        /// </remarks>
        public bool AtEmbedLimit { get { return m_Embeds.Count == HookObject.MAX_EMBEDS; } }

        /// <summary>
        /// Sets the content of the HookObject.
        /// </summary>
        /// <param name="content">The content to set.</param>
        /// <returns>The current instance of <see cref="HookObjectBuilder"/>.</returns>
        public HookObjectBuilder SetContent(string content)
        {
            m_Content = content;
            return this;
        }

        /// <summary>
        /// Adds an embed to the HookObject.
        /// <para>If <see cref="m_Embeds.Count"/> is equal to <see cref="HookObject.MAX_EMBEDS"/> throws <see cref="NotSupportedException"/>.</para>
        /// </summary>
        /// <param name="embed">The <see cref="HookEmbed"/> to add.</param>
        /// <returns>The current instance of <see cref="HookObjectBuilder"/>.</returns>
        /// <exception cref="NotSupportedException">Thrown when trying to add more than the maximum allowed number of embedded files.</exception>
        public HookObjectBuilder AddEmbed(HookEmbed embed)
        {
            if (AtEmbedLimit)
            {
                throw new NotSupportedException($"More than {HookObject.MAX_EMBEDS} Embedded files is not supported on a discord webhook");
            }

            m_Embeds.Add(embed);
            return this;
        }

        /// <summary>
        /// Sets the username for the HookObject being built.
        /// <para>If the provided username is invalid, null or whitespace, or exceeds 80 characters throws a <see cref="ArgumentException"/>.</para>
        /// </summary>
        /// <param name="username">The username to set.</param>
        /// <returns>The current instance of <see cref="HookObjectBuilder"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if the provided username is invalid, null or whitespace, or exceeds 80 characters.</exception>
        public HookObjectBuilder SetUsername(string username)
        {
            if (m_UsernamesInvalidContains.Contains(username.ToLower()))
            {
                throw new ArgumentException("Username cannot contain invalid text");
            }

            if (m_UsernamesInvalidEquals.Contains(username.ToLower()))
            {
                throw new ArgumentException("Username cannot be: " + username);
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be Null Or White Space");
            }

            if (username.Length > 80)
            {
                throw new ArgumentException("Username length cannot be greater than 80");
            }

            m_Username = username;
            return this;
        }

        /// <summary>
        /// An array of strings containing invalid text that should not be part of a username.
        /// </summary>
        readonly string[] m_UsernamesInvalidContains = new string[]
        {
        "discord", "clyde", "system message", "```"
        };

        /// <summary>
        /// An array of strings containing usernames that are considered invalid.
        /// </summary>
        readonly string[] m_UsernamesInvalidEquals = new string[]
        {
        "here", "everyone"
        };

        /// <summary>
        /// Sets the avatar URL for the HookObject.
        /// </summary>
        /// <param name="avatarUrl">The avatar URL to set.</param>
        /// <returns>The current instance of <see cref="HookObjectBuilder"/>.</returns>
        public HookObjectBuilder SetAvatarUrl(string avatarUrl)
        {
            m_AvatarUrl = URiHelper.EazyURi(avatarUrl);
            return this;
        }

        /// <summary>
        /// Sets whether embedded files are disabled for the HookObject.
        /// </summary>
        /// <param name="disabled">A value indicating whether embedded files are disabled.</param>
        /// <returns>The current instance of <see cref="HookObjectBuilder"/>.</returns>
        public HookObjectBuilder SetEmbedsDisabled(bool disabled)
        {
            m_EmbedsDisabled = disabled;
            return this;
        }

        /// <summary>
        /// Builds and returns a <see cref="HookObject"/> instance using the provided properties.
        /// </summary>
        /// <returns>A <see cref="HookObject"/> instance.</returns>
        public HookObject Build()
        {
            return new HookObject(m_Content, m_Embeds, m_Username, m_AvatarUrl, m_EmbedsDisabled);
        }

        /// <summary>
        /// The content of the HookObject.
        /// </summary>
        string m_Content = string.Empty;

        /// <summary>
        /// The list of embedded files in the HookObject.
        /// </summary>
        List<HookEmbed> m_Embeds = new List<HookEmbed>();

        /// <summary>
        /// The username associated with the HookObject.
        /// </summary>
        string m_Username = string.Empty;

        /// <summary>
        /// The avatar URL associated with the HookObject.
        /// </summary>
        Uri m_AvatarUrl = null;

        /// <summary>
        /// A flag indicating whether embedded files are disabled for the HookObject.
        /// </summary>
        bool m_EmbedsDisabled = false;
    }
}