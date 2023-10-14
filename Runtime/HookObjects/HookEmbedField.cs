using Newtonsoft.Json.Linq;
using System;

namespace SI.Discord.Webhooks
{

    /// <summary>
    /// Represents an embed field for use in a hook.
    /// </summary>
    public struct HookEmbedField : IHookContainer
    {
        /// <summary>
        /// Field name, required.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Field value, required.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Indicates whether the field should be displayed inline. Optional.
        /// </summary>
        public bool? Inline { get; private set; }

        /// <summary>
        /// Constructor with validation for required fields.
        /// <para>If either name or value is null or whitespace throws <see cref="ArgumentException"/>.</para>
        /// </summary>
        /// <param name="name">The field name, which cannot be null or whitespace.</param>
        /// <param name="value">The field value, which cannot be null or whitespace.</param>
        /// <param name="inline">A value indicating whether the field should be displayed inline.</param>
        /// <exception cref="ArgumentException">Thrown when either name or value is null or whitespace.</exception>
        public HookEmbedField(string name, string value, bool inline)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
            }

            Name = name;
            Value = value;
            Inline = inline;
        }

        /// <summary>
        /// Converts the HookEmbedField to a JObject.
        /// </summary>
        /// <returns>A JObject representing the HookEmbedField.</returns>
        public JObject ToJObject()
        {
            JObject root = new JObject();
            if (!string.IsNullOrEmpty(Name))
            {
                root.Add(nameof(Name).ToLower(), Name);
            }
            if (!string.IsNullOrEmpty(Value))
            {
                root.Add(nameof(Value).ToLower(), Value);
            }
            if (Inline.HasValue)
            {
                root.Add(nameof(Inline).ToLower(), Inline.Value);
            }
            return root;
        }
    }
}