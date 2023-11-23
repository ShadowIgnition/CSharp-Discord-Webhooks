using Codice.CM.Triggers;
using Newtonsoft.Json.Linq;
using SI.Discord.Webhooks.Utilities;
using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace SI.Discord.Webhooks.Models
{
    // Todo convert to builder?
    public struct HookObjectAttachement : IConvertibleToJObject, IHookObjectToken
    {
        public static Result<string> TryCreate(Uri filepath, out HookObjectAttachement hookEmbedAttachement)
        {
            hookEmbedAttachement = default;
            // Might need multipart/form thingy
            JObject root = new();

            if (filepath == null)
            {
                return "Given Filepath is null";
            }
            else
            {
                JObject attachement = new();

                if (filepath.IsFile)
                {
                    attachement.Add("url", "attachment://" + Path.GetFileName(filepath.AbsoluteUri));
                }
                else
                {
                    attachement.Add("url", filepath.AbsoluteUri);
                }

                root.Add(nameof(File).ToLowerInvariant(), attachement);
            }
            hookEmbedAttachement = new HookObjectAttachement { JObject = root };
            return Result<string>.Success;
        }

        internal JObject JObject { get; private set; }

        /// <summary>
        /// The file URL of the embedded message.
        /// </summary>
        public Uri File { get; private set; }

        public JObject ToJObject()
        {
            return JObject;
        }

        public HookPayload CreatePayload()
        {
            // Initialize a new MultipartFormDataContent
            MultipartFormDataContent formData = new();

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

        public Result<string> Validate()
        {
            return Result<string>.Success;
        }
    }
}