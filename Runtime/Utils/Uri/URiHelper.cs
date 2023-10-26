using System;

namespace SI.Discord.Webhooks
{
    public static class URiHelper
    {
        public static Result<string> TryParseURI(string uri, out Uri result)
        {
            if (string.IsNullOrEmpty(uri))
            {
                result = null;
                return "URI cannot be null or empty";
            }

            if (!Uri.TryCreate(uri, UriKind.Absolute, out result))
            {
                return $"Invalid URI {uri} link";
            }

            return Result<string>.Success;
        }
    }
}