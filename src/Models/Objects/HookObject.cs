using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace SI.Discord.Webhooks.Models
{

    /// <summary>
    /// Represents a structure for creating and handling hook objects.
    /// </summary>
    public struct HookObject : IConvertibleToJObject
    {
        public const int MAX_EMBEDS = 10;
        public const int MAX_USERNAME_LENGTH = 80;

        /// <summary>
        /// Main content of the hook.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// List of embedded content.
        /// </summary>
        public IList<HookEmbed> Embeds { get; private set; }

        /// <summary>
        /// Username associated with the hook.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// URL of the avatar image.
        /// </summary>
        public Uri Avatar_Url { get; private set; }

        /// <summary>
        /// Flags indicating various settings of the hook.
        /// </summary>
        public int Flags { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HookObject"/> struct.
        /// <para>If <see cref="content"/> nor <see cref="embeds"/> is not set, throws <see cref="ArgumentException"/>.</para>
        /// </summary>
        /// <param name="content">The main content of the hook.</param>
        /// <param name="embeds">A list of embedded content.</param>
        /// <param name="username">The username associated with the hook.</param>
        /// <param name="avatarUrl">The URL of the avatar image.</param>
        /// <param name="embedsDisabled">A flag indicating if embeds are disabled.</param>
        /// <exception cref="ArgumentException">Thrown if neither content nor embeds are provided.</exception>
        public HookObject(string content, IList<HookEmbed> embeds, string username, Uri avatarUrl, bool embedsDisabled)
        {
            if (string.IsNullOrEmpty(content) && (embeds == null || embeds.Count == 0))
            {
                throw new ArgumentException("Either Content or Embeds must be provided.");
            }

            Content = content;
            Embeds = embeds;
            Username = username;
            Avatar_Url = avatarUrl;
            Flags = embedsDisabled ? 1 << 2 : 0;
            m_EmbeddedFilesCount = 0;
        }

        /// <summary>
        /// Embeds a file in the form data content.
        /// <para>If <see cref="path"/> is not a valid file path, throws <see cref="FileNotFoundException"/>.</para>
        /// <para>If <see cref="m_EmbeddedFilesCount"/> is equal to <see cref="MAX_EMBEDS"/> throws <see cref="NotSupportedException"/>.</para>
        /// </summary>
        /// <param name="formDataContent">The form data content to which the file will be added.</param>
        /// <param name="path">The URI of the file to embed.</param>
        /// <param name="type">The content type of the file.</param>
        /// <param name="name">The name of the file in the form data.</param>
        /// <exception cref="FileNotFoundException">If <see cref="path"/> is not a valid file path</exception>
        /// <exception cref="NotSupportedException">If <see cref="m_EmbeddedFilesCount"/> is equal to <see cref="MAX_EMBEDS"/></exception>
        public void EmbedFile(MultipartFormDataContent formDataContent, Uri path, string type, string name)
        {
            if (!File.Exists(path.AbsolutePath))
            {
                throw new FileNotFoundException($"Unable to embed file at path {path}, file does not exist!");
            }

            if (m_EmbeddedFilesCount == MAX_EMBEDS)
            {
                throw new NotSupportedException($"More than {MAX_EMBEDS} Embedded files is not supported on a discord webhook");
            }

            byte[] fileBytes;
            using (FileStream fs = new(path.AbsolutePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fileBytes = new byte[fs.Length];
                fs.Read(fileBytes, 0, (int)fs.Length);
            }
            ByteArrayContent fileContent = new(fileBytes);
            fileContent.Headers.Add("Content-Type", type);
            formDataContent.Add(fileContent, $"files[{m_EmbeddedFilesCount}]", name);
            m_EmbeddedFilesCount++;
        }

        /// <summary>
        /// Converts the HookObject to a JSON object.
        /// </summary>
        /// <returns>A JSON object representing the HookObject.</returns>
        public readonly JObject ToJObject()
        {
            JObject root = new();

            if (!string.IsNullOrEmpty(Content))
            {
                root.Add(nameof(Content).ToLower(), Content);
            }

            if (Avatar_Url != null)
            {
                root.Add(nameof(Avatar_Url).ToLower(), Avatar_Url.AbsoluteUri);
            }

            if (Embeds != null && Embeds.Count != 0)
            {
                JArray embeds = new();
                foreach (HookEmbed embed in Embeds)
                {
                    embeds.Add(embed.ToJObject());
                }
                root.Add(nameof(Embeds).ToLower(), embeds);
            }

            if (!string.IsNullOrEmpty(Username))
            {
                root.Add(nameof(Username).ToLower(), Username);
            }

            if (Flags != default)
            {
                root.Add(nameof(Flags).ToLower(), Flags);
            }

            return root;
        }

        /// <summary>
        /// The count of embedded files.
        /// </summary>
        int m_EmbeddedFilesCount;
    }
}