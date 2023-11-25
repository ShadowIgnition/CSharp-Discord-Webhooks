using NUnit.Framework;
using SI.Discord.Webhooks.Models;
using SI.Discord.Webhooks.Utilities;
using System;
using System.Drawing;
using System.IO;

namespace SI.Discord.Webhooks.Tests
{
    [TestFixture]
    public class HookEmbedBuilderTests
    {
        [Test]
        public void SetTitle_SetsTitle()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            string title = "Test Title";
            builder.SetTitle(title);
            HookEmbed embed = builder.Build();

            Assert.AreEqual(title, embed.Title);
        }

        [Test]
        public void SetDescription_SetsDescription()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            string description = "Test Description";
            builder.SetDescription(description);
            HookEmbed embed = builder.Build();

            Assert.AreEqual(description, embed.Description);
        }

        [Test]
        public void SetColorWithColor_SetsColor()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            Color color = Color.Red;
            builder.SetColor(color);
            HookEmbed embed = builder.Build();

            Assert.AreEqual(color.ToInteger(), embed.Color);
        }

        [Test]
        public void SetColorWithInteger_SetsColor()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            int color = 16711680; // Red color as integer
            builder.SetColor(color);
            HookEmbed embed = builder.Build();

            Assert.AreEqual(color, embed.Color);
        }

        [Test]
        public void AddField_AddsField()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            HookEmbedField field = new HookEmbedField("Test Field", "Test Value", false);
            builder.AddField(field);
            HookEmbed embed = builder.Build();

            Assert.Contains(field, (System.Collections.ICollection)embed.Fields);
        }

        [Test]
        public void SetAuthor_SetsAuthor()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            HookEmbedAuthor author = new HookEmbedAuthor("Test Author", "Test URL", "Test IconUrl");
            builder.SetAuthor(author);
            HookEmbed embed = builder.Build();

            Assert.AreEqual(author, embed.Author);
        }

        [Test]
        public void SetText_SetsText()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            string text = "Test Text";
            builder.SetText(text);
            HookEmbed embed = builder.Build();

            Assert.AreEqual(text, embed.Footer);
        }

        [Test]
        public void SetTimestamp_SetsTimestamp()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            DateTime timestamp = DateTime.Now;
            builder.SetTimestamp(timestamp);
            HookEmbed embed = builder.Build();

            Assert.AreEqual(timestamp, embed.Timestamp);
        }

        [Test]
        public void TrySetURL_ValidURL_ReturnsSuccessResult()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            string url = "https://example.com";
            Result<string> result = builder.TrySetURL(url);

            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public void TrySetURL_InvalidURL_ReturnsFailureResult()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            string url = "invalid_url";
            Result<string> result = builder.TrySetURL(url);

            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public void TrySetImageURL_ValidURL_ReturnsSuccessResult()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            string url = IMAGE_URL;
            Result<string> result = builder.TrySetImageURL(url);

            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public void TrySetThumbnailURL_ValidURL_ReturnsSuccessResult()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            string url = IMAGE_URL;
            Result<string> result = builder.TrySetThumbnailURL(url);

            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public void TrySetFileURL_ValidURL_ReturnsSuccessResult()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            Result<string> result = builder.TrySetFileURL(Path.GetTempFileName());

            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public void TrySetFileURL_InvalidURL_ReturnsFailureResult()
        {
            HookEmbedBuilder builder = new HookEmbedBuilder();
            string url = "invalid_url";
            Result<string> result = builder.TrySetFileURL(url);

            Assert.IsFalse(result.Succeeded);
        }

        const string IMAGE_URL = "https://github.com/ShadowIgnition/CSharp-Discord-Webhooks/blob/development/Tests/image.png";
    }
}
