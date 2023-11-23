using NUnit.Framework;
using SI.Discord.Webhooks.Models;
using SI.Discord.Webhooks.Utilities;
using System;

namespace SI.Discord.Webhooks.Tests
{
    [TestFixture]
    public class HookObjectBuilderTests
    {
        [Test]
        public void SetContent_ValidContent_ContentIsSet()
        {
            const string Content = "Test Content";

            HookObjectBuilder builder = new();

            builder.SetContent(Content);

            Assert.AreEqual(Content, builder.Build().Content);
        }

        [Test]
        public void TryAddEmbed_LessThanLimit_EmbedAddedSuccessfully()
        {
            HookObjectBuilder builder = new();
            HookEmbedContent embed = new();

            Result<string> result = builder.TryAddEmbed(embed);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(1, builder.Build().Embeds.Count);
        }

        [Test]
        public void TryAddEmbed_AtLimit_ErrorReturned()
        {
            HookObjectBuilder builder = new();
            HookEmbedContent embed = new();

            // Add maximum number of embeds
            for (int i = 0; i <= HookObject.MAX_EMBEDS; i++)
            {
                builder.TryAddEmbed(embed);
            }

            Result<string> result = builder.TryAddEmbed(embed);

            Assert.IsTrue(result.Failed);
        }

        [Test]
        public void TrySetUsername_InvalidText_ErrorReturned()
        {
            HookObjectBuilder builder = new();

            Result<string> result = builder.TrySetUsername("discord username");

            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public void TrySetUsername_InvalidUsername_ErrorReturned()
        {
            HookObjectBuilder builder = new();

            Result<string> result = builder.TrySetUsername("here");

            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public void TrySetUsername_NullOrWhiteSpace_ErrorReturned()
        {
            HookObjectBuilder builder = new();

            Result<string> result = builder.TrySetUsername(null);

            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public void TrySetUsername_LongUsername_ErrorReturned()
        {
            HookObjectBuilder builder = new();

            Result<string> result = builder.TrySetUsername(new string('x', 81));

            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public void TrySetAvatarUrl_ValidUrl_UrlIsSet()
        {
            HookObjectBuilder builder = new();
            builder.SetContent("Content");
            string url = IMAGE_URL;

            Result<string> result = builder.TrySetAvatarUrl(url);

            Assert.IsTrue(result.Succeeded);
            Uri a = new(url);
            Uri b = builder.Build().Avatar_Url;
            Assert.AreEqual(a, b);
        }

        [Test]
        public void TrySetAvatarUrl_InvalidUrl_ErrorReturned()
        {

            HookObjectBuilder builder = new();
            string invalidUrl = "invalid-url";


            Result<string> result = builder.TrySetAvatarUrl(invalidUrl);


            Assert.IsFalse(result.Succeeded);
            Assert.IsNotNull(result.Message);
        }

        const string IMAGE_URL = "https://github.com/ShadowIgnition/CSharp-Discord-Webhooks/blob/development/Tests/image.png";
    }

}
