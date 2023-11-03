using SI.Discord.Webhooks.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SI.Discord.Webhooks.Models
{
    /// <summary>
    /// This class represents a builder for creating a Discord webhook message object.
    /// It provides methods for setting various properties of the message.
    /// </summary>
    public class HookObjectBuilder
    {
        /// <summary>
        /// Gets a value indicating whether the number of embeds has reached the maximum limit.
        /// </summary>
        public bool AtEmbedLimit { get { return m_Embeds.Count == HookObject.MAX_EMBEDS; } }

        /// <summary>
        /// Sets the content of the message.
        /// </summary>
        /// <param name="content">The content of the message.</param>
        /// <returns>The modified HookObjectBuilder instance.</returns>
        public HookObjectBuilder SetContent(string content)
        {
            m_Content = content;
            return this;
        }

        /// <summary>
        /// Tries to add an embed to the message.
        /// </summary>
        /// <param name="embed">The embed to be added.</param>
        /// <returns>
        /// A Result containing a success message if the operation was successful,
        /// or an error message if the operation failed.
        /// </returns>
        public Result<string> TryAddEmbed(HookEmbed embed)
        {
            if (AtEmbedLimit)
            {
                return $"More than {HookObject.MAX_EMBEDS} Embedded files is not supported on a discord webhook";
            }

            if (m_EmbedsDisabled)
            {
                return $"Embeds disabled, re-enable to add embeds";
            }

            m_Embeds.Add(embed);
            return Result<string>.Success;
        }

        /// <summary>
        /// Tries to set the username for the message.
        /// </summary>
        /// <param name="username">The desired username.</param>
        /// <returns>
        /// A Result containing a success message if the operation was successful,
        /// or an error message if the operation failed.
        /// </returns>
        public Result<string> TrySetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return "Username cannot be Null Or White Space";
            }

            foreach (string substring in m_UsernamesInvalidContains)
            {
                if (username.ToLower().Contains(substring.ToLower()))
                {
                    return "Username cannot contain invalid text: " + substring;
                }
            }

            if (m_UsernamesInvalidEquals.Contains(username.ToLower()))
            {
                return "Username cannot be: " + username;
            }

            if (username.Length > 80)
            {
                return "Username length cannot be greater than 80";
            }

            m_Username = username;
            return Result<string>.Success;
        }

        /// <summary>
        /// Tries to set the avatar URL for the message.
        /// </summary>
        /// <param name="avatarUrl">The URL of the avatar.</param>
        /// <returns>
        /// A Result containing a success message if the operation was successful,
        /// or an error message if the operation failed.
        /// </returns>
        public Result<string> TrySetAvatarUrl(string avatarUrl)
        {
            Result<string> result = URiUtils.TryParseURI(avatarUrl, out Uri resultURI);
            m_AvatarUrl = resultURI;
            return result;
        }

        /// <summary>
        /// Sets whether embeds are disabled for the message.
        /// </summary>
        /// <param name="disabled">A boolean indicating whether embeds are disabled.</param>
        /// <returns>The modified HookObjectBuilder instance.</returns>
        public HookObjectBuilder SetEmbedsDisabled(bool disabled)
        {
            m_EmbedsDisabled = disabled;
            return this;
        }

        /// <summary>
        /// Builds and returns the final HookObject based on the set properties.
        /// </summary>
        /// <returns>The constructed HookObject.</returns>
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
        readonly List<HookEmbed> m_Embeds = new();

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
    }
}
