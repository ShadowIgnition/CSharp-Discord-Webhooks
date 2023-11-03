using NUnit.Framework;
using SI.Discord.Webhooks.Models;
using SI.Discord.Webhooks.Utilities;

namespace SI.Discord.Webhooks.Tests
{
    [TestFixture]
    public class HookObjectValidatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_Validator = new HookObjectValidator();
        }

        [Test]
        public void HasValidUsername_ShouldReturnSuccessForValidUsername()
        {
            HookObjectBuilder hookObjectBuilder = new HookObjectBuilder();
            hookObjectBuilder.TrySetUsername("ValidUsername");
            hookObjectBuilder.SetContent("content");

            Result<string> result = m_Validator.HasValidUsername(hookObjectBuilder.Build());

            Assert.AreEqual(Result<string>.Success, result);
        }

        [Test]
        public void HasValidUsername_ShouldAllowEmptyUsername()
        {
            HookObjectBuilder hookObjectBuilder = new HookObjectBuilder();
            hookObjectBuilder.TrySetUsername(string.Empty);
            hookObjectBuilder.SetContent("content");

            Result<string> result = m_Validator.HasValidUsername(hookObjectBuilder.Build());

            Assert.AreEqual(Result<string>.Success, result);
        }

        [Test]
        public void HasValidUsername_ShouldRejectRestrictedSubstring()
        {
            HookObjectBuilder hookObjectBuilder = new HookObjectBuilder();
            hookObjectBuilder.TrySetUsername("User#Name");
            hookObjectBuilder.SetContent("content");

            Result<string> result = m_Validator.HasValidUsername(hookObjectBuilder.Build());

            Assert.AreNotEqual(Result<string>.Success, result);
        }

        HookObjectValidator m_Validator;
    }

}
