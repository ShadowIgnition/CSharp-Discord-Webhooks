using System.Data.SqlTypes;
using System.Linq;

namespace SI.Discord.Webhooks
{
    public class HookObjectValidator
    {
        public bool ReasonAIsValid()
        {
            return true;
        }

        public bool ExceedsEmbedLimit(HookObject hookObject)
        {
            return hookObject.Embeds.Count <= HookObject.MAX_EMBEDS;
        }

        /// <summary>
        /// https://discord.com/developers/docs/resources/user#usernames-and-nicknames
        /// </summary>
        public bool HasValidUsername(HookObject hookObject, out string reason)
        {
            string username = hookObject.Username.ToLowerInvariant();

            // Null or empty usernames won't get processed and will use the default webhook name
            if (string.IsNullOrEmpty(username))
            {
                reason = string.Empty;
                return true;
            }

            // Usernames must be no longer than MAX_LENGTH
            if (username.Length > MAX_LENGTH)
            {
                reason = "Username must be no longer than " + MAX_LENGTH;
                return false;
            }

            // Usernames cannot contain restricted substrings.
            foreach (string substring in m_RestrictedSubstrings)
            {
                if (username.Contains(substring))
                {
                    reason = "Username cannot contain restricted substring " + substring;
                    return false;
                }
            }

            // Usernames cannot be in the restricted list.
            if (m_RestrictedUsernames.Contains(username))
            {
                reason = "Username cannot be " + username;
                return false;
            }

            reason = string.Empty;
            return true;
        }

        const int MAX_LENGTH = 80;

        readonly string[] m_RestrictedUsernames = new string[]
        {
            "everyone", "here"
        };

        readonly string[] m_RestrictedSubstrings = new string[]
        {
            "@", "#", ":", "```", "discord", "clyde"
        };
    }
}
