using SI.Discord.Webhooks.Utilities;
using System.Linq;

namespace SI.Discord.Webhooks.Models
{
    /// <summary>
    /// This class provides validation methods for a HookObject.
    /// </summary>
    public class HookObjectValidator
    {
        /// <summary>
        /// Checks if the number of embeds in the HookObject exceeds the maximum allowed limit.
        /// </summary>
        /// <param name="hookObject">The HookObject to validate.</param>
        /// <returns>True if the number of embeds is within the limit, otherwise false.</returns>
        public bool WithinEmbedLimit(HookObject hookObject)
        {
            return hookObject.Embeds.Count <= HookObject.MAX_EMBEDS;
        }

        /// <summary>
        /// Validates the username of a HookObject.
        /// </summary>
        /// <param name="hookObject">The HookObject to validate.</param>
        /// <returns>
        /// A Result indicating the validation result.
        /// </returns>
        public Result<string> HasValidUsername(HookObject hookObject)
        {
            string username = hookObject.Username.ToLowerInvariant();

            // Null or empty usernames won't get processed and will use the default webhook name
            if (string.IsNullOrEmpty(username))
            {
                return Result<string>.Success;
            }

            // Usernames must be no longer than MAX_LENGTH
            if (username.Length > MAX_LENGTH)
            {
                return "Username must be no longer than " + MAX_LENGTH;
            }

            // Usernames cannot contain restricted substrings.
            foreach (string substring in m_RestrictedSubstrings)
            {
                if (username.Contains(substring))
                {
                    return "Username cannot contain restricted substring " + substring;
                }
            }

            // Usernames cannot be in the restricted list.
            if (m_RestrictedUsernames.Contains(username))
            {
                return "Username cannot be " + username;
            }

            return Result<string>.Success;
        }

        // Maximum allowed length for a username
        const int MAX_LENGTH = 80;

        // List of restricted usernames
        readonly string[] m_RestrictedUsernames = new string[]
        {
            "everyone", "here"
        };

        // List of restricted substrings in usernames
        readonly string[] m_RestrictedSubstrings = new string[]
        {
            "@", "#", ":", "```", "discord", "clyde"
        };
    }
}
