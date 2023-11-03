namespace SI.Discord.Webhooks.Models
{
    /// <summary>
    /// Represents a Discord Webhook object.
    /// </summary>
    public record WebhookObject
    {
        /// <summary>
        /// Constructor for creating a WebhookObject.
        /// </summary>
        /// <param name="id">The id of the webhook.</param>
        /// <param name="type">The type of the webhook.</param>
        /// <param name="guildId">The guild id this webhook is for, if any.</param>
        /// <param name="channelId">The channel id this webhook is for, if any.</param>
        /// <param name="name">The default name of the webhook.</param>
        /// <param name="avatar">The default user avatar hash of the webhook.</param>
        /// <param name="token">The secure token of the webhook (returned for Incoming Webhooks).</param>
        /// <param name="applicationId">The bot/OAuth2 application that created this webhook.</param>
        /// <param name="url">The url used for executing the webhook (returned by the webhooks OAuth2 flow).</param>
        public WebhookObject(ulong id, int type, ulong? guildId, ulong channelId, string name, string avatar, string token, ulong? applicationId, string url)
        {
            Id = id;
            Type = type;
            GuildId = guildId;
            ChannelId = channelId;
            Name = name;
            Avatar = avatar;
            Token = token;
            ApplicationId = applicationId;
            Url = url;
        }

        /// <summary>
        /// Gets the id of the webhook.
        /// </summary>
        public ulong Id { get; }

        /// <summary>
        /// Gets the type of the webhook.
        /// </summary>
        public int Type { get; }

        /// <summary>
        /// Gets the guild id this webhook is for, if any.
        /// </summary>
        public ulong? GuildId { get; }

        /// <summary>
        /// Gets the channel id this webhook is for, if any.
        /// </summary>
        public ulong ChannelId { get; }

        // Unimplemented properties:
        //public User User { get; set; } // the user this webhook was created by (not returned when getting a webhook with its token)
        //public PartialGuild SourceGuild { get; } // the guild of the channel that this webhook is following (returned for Channel Follower Webhooks)
        //public PartialChannel SourceChannel { get; } // the channel that this webhook is following (returned for Channel Follower Webhooks)

        /// <summary>
        /// Gets the default name of the webhook.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the default user avatar hash of the webhook.
        /// </summary>
        public string Avatar { get; }

        /// <summary>
        /// Gets the secure token of the webhook (returned for Incoming Webhooks).
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// Gets the bot/OAuth2 application that created this webhook.
        /// </summary>
        public ulong? ApplicationId { get; }

        /// <summary>
        /// Gets the url used for executing the webhook (returned by the webhooks OAuth2 flow).
        /// </summary>
        public string Url { get; }
    }
}
