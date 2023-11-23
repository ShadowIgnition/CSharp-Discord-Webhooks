using Newtonsoft.Json.Linq;
using SI.Discord.Webhooks.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SI.Discord.Webhooks.Models
{
    public interface IHookObjectToken
    {
        /// <summary>
        /// Creates the payload for the webhook request.
        /// </summary>
        /// <returns>A <see cref="HttpContent"/> object representing the payload.</returns>
        public HookPayload CreatePayload();

        public Result<string> Validate();
    }


    /// <summary>
    /// Represents a structure for creating and handling hook objects.
    /// </summary>
    public struct HookObject : IHookObjectToken, IConvertibleToJObject
    {
        public const int MAX_EMBEDS = 10;
        public const int MAX_USERNAME_LENGTH = 80;

        /// <summary>
        /// Main content of the hook.
        /// </summary>
        public string Content { get; private set; }

        public string Thread_Name;

        /// <summary>
        /// List of embedded content.
        /// </summary>
        public IList<HookEmbedContent> Embeds { get; private set; }

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
        public HookObject(string content, IList<HookEmbedContent> embeds, string username, Uri avatarUrl, bool embedsDisabled, string a)
        {
            if (string.IsNullOrEmpty(content) && (embeds == null || embeds.Count == 0))
            {
                throw new ArgumentException("Either Content or Embeds must be provided.");
            }
            Thread_Name = a;
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
        public void SetEmbeddedFile(MultipartFormDataContent formDataContent, Uri path, string type, string name)
        {
            if (!File.Exists(path.LocalPath))
            {
                throw new FileNotFoundException($"Unable to embed file at path {path}, file does not exist!");
            }

            if (m_EmbeddedFilesCount == MAX_EMBEDS)
            {
                throw new NotSupportedException($"More than {MAX_EMBEDS} Embedded files is not supported on a discord webhook");
            }

            byte[] fileBytes;
            using (FileStream fs = new(path.LocalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
        /// Creates the payload for the webhook request.
        /// </summary>
        /// <returns>A <see cref="HttpContent"/> object representing the payload.</returns>
        public HookPayload CreatePayload()
        {
            // Initialize a new MultipartFormDataContent
            MultipartFormDataContent formData = new();

            // Can only upload 1 embed per items
            foreach (HookEmbedContent hookEmbed in Embeds)
            {
                TryEmbedThumbnail(this, formData, hookEmbed);
            }

            // Create a JSON object structure from the HookObject
            JObject json = ToJObject();

            // Convert the JSON object to a string
            string jsonStr = json.ToString();

            // Create a StringContent with JSON data
            StringContent jsonContent = new(jsonStr, Encoding.UTF8, "application/json");

            // Add JSON content to formData
            formData.Add(jsonContent, "payload_json");

            // Return the final payload
            return new(formData);
        }

        bool TryEmbedThumbnail(HookObject hookObject, MultipartFormDataContent formData, HookEmbedContent embed)
        {
            // Check if the embed contains a thumbnail file
            if (embed.Thumbnail != null && embed.Thumbnail.IsFile)
            {
                // Add the thumbnail file to the form data
                hookObject.SetEmbeddedFile(formData, embed.Thumbnail, "image/png", Path.GetFileName(embed.Thumbnail.LocalPath));
                return true;
            }
            return false;
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
                root.Add(nameof(Content).ToLowerInvariant(), Content);
            }

            if (Avatar_Url != null)
            {
                root.Add(nameof(Avatar_Url).ToLowerInvariant(), Avatar_Url.AbsoluteUri);
            }

            if (Embeds != null && Embeds.Count != 0)
            {
                JArray embeds = new();
                foreach (HookEmbedContent embed in Embeds)
                {
                    embeds.Add(embed.ToJObject());
                }
                root.Add(nameof(Embeds).ToLowerInvariant(), embeds);
            }

            if (!string.IsNullOrEmpty(Username))
            {
                root.Add(nameof(Username).ToLowerInvariant(), Username);
            }

            if (Flags != default)
            {
                root.Add(nameof(Flags).ToLowerInvariant(), Flags);
            }
            root.Add(nameof(Thread_Name).ToLowerInvariant(), Thread_Name); ;

            return root;
        }

        public readonly Result<string> Validate()
        {
            bool passedValidation = true;
            List<string> failureReasons = new();
            HookObjectValidator validator = new();

            if (!validator.WithinEmbedLimit(this))
            {
                passedValidation = false;
                failureReasons.Add($"More than {MAX_EMBEDS} Embedded files is not supported on a discord webhook");
            }

            Result<string> userNameResult = validator.HasValidUsername(this);

            if (!userNameResult.Succeeded)
            {
                passedValidation = false;
                failureReasons.Add("Invalid Username: " + userNameResult.Message);
            }

            if (!passedValidation)
            {
                string reasons = string.Join(", ", failureReasons);
                return $"An error occurred while sending the webhook, reasons: {reasons}";
            }

            return Result<string>.Success;
        }

        /// <summary>
        /// The count of embedded files.
        /// </summary>
        int m_EmbeddedFilesCount;
    }
}