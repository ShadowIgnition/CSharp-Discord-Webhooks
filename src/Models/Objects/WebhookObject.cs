namespace SI.Discord.Webhooks.Models
{
    public record WebhookObject
    {
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

        public ulong Id { get; } // the id of the webhook
        public int Type { get; } // the type of the webhook
        public ulong? GuildId { get;  } // the guild id this webhook is for, if any
        public ulong ChannelId { get;  } // the channel id this webhook is for, if any
        //public User User { get;  } // the user this webhook was created by (not returned when getting a webhook with its token)
        public string Name { get;  } // the default name of the webhook
        public string Avatar { get;  } // the default user avatar hash of the webhook
        public string Token { get;  } // the secure token of the webhook (returned for Incoming Webhooks)
        public ulong? ApplicationId { get;  } // the bot/OAuth2 application that created this webhook
        //public PartialGuild SourceGuild { get;  } // the guild of the channel that this webhook is following (returned for Channel Follower Webhooks)
        //public PartialChannel SourceChannel { get;  } // the channel that this webhook is following (returned for Channel Follower Webhooks)
        public string Url { get;  } // the url used for executing the webhook (returned by the webhooks OAuth2 flow)
    }
}