using System;

namespace SI.Discord.Webhooks
{
    public static class URiHelper
    {
        public static Uri EazyURi(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                Console.WriteLine("Not a url: " + url);
            }

            return uri;
        }
    }
}