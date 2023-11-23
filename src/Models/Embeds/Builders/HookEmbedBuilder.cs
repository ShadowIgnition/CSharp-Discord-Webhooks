using SI.Discord.Webhooks.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SI.Discord.Webhooks.Models
{
    /// <summary>
    /// A builder class for creating Discord webhook embeds.
    /// </summary>
    public class HookEmbedBuilder
    {
        /// <summary>
        /// Builds and returns the final HookEmbed instance.
        /// </summary>
        /// <returns>The built HookEmbed instance.</returns>
        public HookEmbedContent Build()
        {
            return new HookEmbedContent(Title, Description, m_Url, m_Color, m_Fields, m_Author, m_Text, m_Timestamp, m_ThumbnailURL);
        }

        /// <summary>
        /// Sets the title of the embed.
        /// </summary>
        /// <param name="title">The title to set.</param>
        /// <returns>The current instance of <see cref="HookEmbedBuilder"/>.</returns>
        public HookEmbedBuilder SetTitle(string title)
        {
            Title = title;
            return this;
        }

        /// <summary>
        /// Sets the description of the embed.
        /// </summary>
        /// <param name="description">The description to set.</param>
        /// <returns>The current instance of <see cref="HookEmbedBuilder"/>.</returns>
        public HookEmbedBuilder SetDescription(string description)
        {
            Description = description;
            return this;
        }

        /// <summary>
        /// Sets the color of the embed.
        /// </summary>
        /// <param name="color">The color to set.</param>
        /// <returns>The current instance of <see cref="HookEmbedBuilder"/>.</returns>
        public HookEmbedBuilder SetColor(System.Drawing.Color color)
        {
            m_Color = color.ToInteger();
            return this;
        }

        /// <summary>
        /// Sets the color of the embed.
        /// </summary>
        /// <param name="color">The color to set.</param>
        /// <returns>The current instance of <see cref="HookEmbedBuilder"/>.</returns>
        public HookEmbedBuilder SetColor(Color color)
        {
            m_Color = color.ToInteger();
            return this;
        }

        /// <summary>
        /// Sets the color of the embed using an integer value.
        /// </summary>
        /// <param name="color">The color to set as an integer.</param>
        /// <returns>The current instance of <see cref="HookEmbedBuilder"/>.</returns>
        public HookEmbedBuilder SetColor(int? color)
        {
            m_Color = color;
            return this;
        }

        /// <summary>
        /// Adds a field to the embed.
        /// </summary>
        /// <param name="field">The field to add.</param>
        /// <returns>The current instance of <see cref="HookEmbedBuilder"/>.</returns>
        public HookEmbedBuilder AddField(HookEmbedField field)
        {
            m_Fields ??= new List<HookEmbedField>();
            m_Fields.Add(field);
            return this;
        }

        /// <summary>
        /// Sets the author of the embed.
        /// </summary>
        /// <param name="author">The author to set.</param>
        /// <returns>The current instance of <see cref="HookEmbedBuilder"/>.</returns>
        public HookEmbedBuilder SetAuthor(HookEmbedAuthor author)
        {
            m_Author = author;
            return this;
        }

        /// <summary>
        /// Sets the text content of the embed.
        /// </summary>
        /// <param name="text">The text content to set.</param>
        /// <returns>The current instance of <see cref="HookEmbedBuilder"/>.</returns>
        public HookEmbedBuilder SetText(string text)
        {
            m_Text = text;
            return this;
        }

        /// <summary>
        /// Sets the timestamp of the embed.
        /// </summary>
        /// <param name="dateTime">The timestamp to set.</param>
        /// <returns>The current instance of <see cref="HookEmbedBuilder"/>.</returns>
        public HookEmbedBuilder SetTimestamp(DateTime dateTime)
        {
            m_Timestamp = dateTime;
            return this;
        }

        /// <summary>
        /// Sets the URL of the embed.
        /// </summary>
        /// <param name="url">The URL to set.</param>
        /// <returns>The current instance of <see cref="HookEmbedBuilder"/>.</returns>
        public Result<string> TrySetURL(string url)
        {
            return URiUtils.TryParseURI(url, out m_Url);
        }

        /// <summary>
        /// Sets the thumbnail URL of the embed.
        /// </summary>
        /// <param name="thumbnailURL">The thumbnail URL to set.</param>
        /// <returns>The current instance of <see cref="HookEmbedBuilder"/>.</returns>
        public Result<string> TrySetThumbnailURL(string thumbnailURL)
        {
            return URiUtils.TryParseURI(thumbnailURL, out m_ThumbnailURL);
        }

        /// <summary>
        /// The title of the embed.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The description of the embed.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The URL associated with the embed.
        /// </summary>
        Uri m_Url;

        /// <summary>
        /// The color of the embed.
        /// </summary>
        int? m_Color;

        /// <summary>
        /// The list of fields to be included in the embed.
        /// </summary>
        List<HookEmbedField> m_Fields;

        /// <summary>
        /// The author information for the embed.
        /// </summary>
        HookEmbedAuthor? m_Author;

        /// <summary>
        /// The text content of the embed.
        /// </summary>
        string m_Text;

        /// <summary>
        /// The timestamp for the embed.
        /// </summary>
        DateTime? m_Timestamp;

        /// <summary>
        /// The URL for the thumbnail image in the embed.
        /// </summary>
        Uri m_ThumbnailURL;

        /// <summary>
        /// The URL for a file associated with the embed.
        /// </summary>
        Uri m_FileURL;
    }
}