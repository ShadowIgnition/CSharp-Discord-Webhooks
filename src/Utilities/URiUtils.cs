using System;

namespace SI.Discord.Webhooks.Utilities
{
    /// <summary>
    /// Contains utility methods for working with URIs.
    /// </summary>
    public static class URiUtils
    {
        /// <summary>
        /// Tries to parse the provided string as a URI.
        /// </summary>
        /// <param name="uri">The string representation of the URI to parse.</param>
        /// <param name="result">When successful, contains the parsed URI.</param>
        /// <returns>
        /// A <see cref="Result{string}"/> indicating the outcome of the parsing operation.
        /// If successful, the result will be a string indicating success.
        /// If unsuccessful, the result will contain an error message.
        /// </returns>
        public static Result<string> TryParseURI(string uri, out Uri result)
        {
            // Check if the provided URI string is null or empty.
            if (string.IsNullOrEmpty(uri))
            {
                result = null;
                return "URI cannot be null or empty";
            }

            // Attempt to create a URI from the provided string.
            if (!Uri.TryCreate(uri, UriKind.Absolute, out result))
            {
                // Return an error message if URI creation fails.
                return $"Invalid URI {uri} link";
            }

            // Return a success message if URI creation is successful.
            return Result<string>.Success;
        }
    }
}
