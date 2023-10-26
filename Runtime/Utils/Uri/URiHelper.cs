using System;

namespace SI.Discord.Webhooks
{
    public static class URiHelper
    {
        public static Result<Exception> TryParseURI(string uri, out Uri result)
        {
            if (string.IsNullOrEmpty(uri))
            {
                result = null;
                return new ArgumentNullException("URI cannot be null or empty");
            }

            if (!Uri.TryCreate(uri, UriKind.Absolute, out result))
            {
                return new Exception($"Invalid URI {uri} link");
            }

            return Result<Exception>.Success;
        }
    }
}