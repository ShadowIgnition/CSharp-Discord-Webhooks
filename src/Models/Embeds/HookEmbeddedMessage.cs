using GluonGui.Dialog;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace SI.Discord.Webhooks.Models
{

    /// <summary>
    /// Represents an embedded message hook container.
    /// </summary>
    public struct HookEmbedContent : IConvertibleToJObject
    {
        /// <summary>
        /// The title of the embedded message.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The description of the embedded message.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The URL associated with the embedded message.
        /// </summary>
        public Uri URL { get; private set; }

        /// <summary>
        /// The color of the embedded message.
        /// </summary>
        public int? Color { get; private set; }

        /// <summary>
        /// The list of fields in the embedded message.
        /// </summary>
        public ICollection<HookEmbedField> Fields { get; private set; }

        /// <summary>
        /// The author of the embedded message.
        /// </summary>
        public HookEmbedAuthor? Author { get; private set; }

        /// <summary>
        /// Footer text, optional.
        /// </summary>
        public string Footer { get; private set; }

        /// <summary>
        /// The timestamp of the embedded message.
        /// </summary>
        public DateTime? Timestamp { get; private set; }

        /// <summary>
        /// The thumbnail URL of the embedded message.
        /// </summary>
        public Uri Thumbnail { get; private set; }

        /// <summary>
        /// Initializes a new instance of the HookEmbed struct.
        /// </summary>
        /// <param name="title">The title of the embedded message.</param>
        /// <param name="description">The description of the embedded message.</param>
        /// <param name="url">The URL associated with the embedded message.</param>
        /// <param name="color">The color of the embedded message.</param>
        /// <param name="fields">The collection of fields in the embedded message.</param>
        /// <param name="author">The author of the embedded message.</param>
        /// <param name="text">The footer text of the embedded message.</param>
        /// <param name="timestamp">The timestamp of the embedded message.</param>
        /// <param name="image">The image URL of the embedded message.</param>
        /// <param name="thumbnail">The thumbnail URL of the embedded message.</param>
        /// <param name="file">The file URL of the embedded message.</param>
        public HookEmbedContent(string title, string description, Uri url, int? color, ICollection<HookEmbedField> fields, HookEmbedAuthor? author, string text, DateTime? timestamp, Uri thumbnail)
        {
            Title = title;
            Description = description;
            URL = url;
            Color = color;
            Fields = fields;
            Author = author;
            Footer = text;
            Timestamp = timestamp;
            Thumbnail = thumbnail;
        }

        /// <summary>
        /// Converts the HookEmbed object to a JObject.
        /// </summary>
        /// <returns>The JObject representing the HookEmbed object.</returns>
        public readonly JObject ToJObject()
        {
            JObject root = new();

            if (!string.IsNullOrEmpty(Title))
            {
                root.Add(nameof(Title).ToLowerInvariant(), Title);
            }

            if (!string.IsNullOrEmpty(Description))
            {
                root.Add(nameof(Description).ToLowerInvariant(), Description);
            }

            if (URL != null)
            {
                root.Add(nameof(URL).ToLowerInvariant(), URL);
            }

            if (Color.HasValue)
            {
                root.Add(nameof(Color).ToLowerInvariant(), Color);
            }

            if (!string.IsNullOrEmpty(Footer))
            {
                JObject footer = new()
                {
                { "text", Footer }
            };
                root.Add(nameof(Footer).ToLowerInvariant(), footer);
            }

            if (Timestamp.HasValue)
            {
                // Convert DateTime to ISO 8601 format
                string iso8601DateTime = Timestamp.Value.ToString(ISO8601);
                root.Add(nameof(Timestamp).ToLowerInvariant(), iso8601DateTime);
            }

            if (Fields != null && Fields.Count != 0)
            {
                JArray fields = new();
                foreach (HookEmbedField field in Fields)
                {
                    fields.Add(field.ToJObject());
                }
                root.Add(nameof(Fields).ToLowerInvariant(), fields);
            }

            if (Author.HasValue)
            {
                root.Add(nameof(Author).ToLowerInvariant(), Author.Value.ToJObject());
            }


            if (Thumbnail != null)
            {
                JObject thumbnail = new();

                if (Thumbnail.IsFile)
                {
                    thumbnail.Add("url", "attachment://" + Path.GetFileName(Thumbnail.AbsoluteUri));
                }
                else
                {
                    thumbnail.Add("url", Thumbnail.AbsoluteUri);
                }

                root.Add(nameof(Thumbnail).ToLowerInvariant(), thumbnail);
            }

            return root;
        }

        /// <summary>
        /// The format string for ISO 8601 date-time representation.
        /// </summary>
        const string ISO8601 = "yyyy-MM-ddTHH:mm:ss.fffZ";
    }
}