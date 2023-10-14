﻿using Newtonsoft.Json.Linq;

namespace SI.Discord.Webhooks
{

    /// <summary>
    /// Represents an interface for objects that can be converted to a <see cref="JObject"/>.
    /// </summary>
    public interface IHookContainer
    {
        /// <summary>
        /// Converts the implementing object to a <see cref="JObject"/>.
        /// </summary>
        /// <returns>A <see cref="JObject"/> representation of the object.</returns>
        JObject ToJObject();
    }
}