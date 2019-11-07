using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PushBulletSharp.Core.Tests
{
    [TestClass]
    public class OAuthTests : TestBase
    {
        [TestMethod]
        public void RequestTokenTest()
        {
            try
            {
                Models.Requests.OAuthTokenRequest request = new Models.Requests.OAuthTokenRequest()
                {
                    ClientId = TestHelper.GetConfig("OAuth:ClientId"),
                    ClientSecret = TestHelper.GetConfig("OAuth:ClientSecret"),
                    Code = TestHelper.GetConfig("OAuth:Code")
                };

                var result = Client.RequestToken(request);

                Client.AccessToken = result.AccessToken;
                var oauthUserInformation = Client.CurrentUsersInformation();
                var oauthTestPushResults = Client.PushNote(new Models.Requests.PushNoteRequest()
                {
                    Title = "OAuth Push Test",
                    Body = "This is a test push using OAuth!",
                    Email = oauthUserInformation.Email
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
