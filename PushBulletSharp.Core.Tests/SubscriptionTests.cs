using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PushBulletSharp.Core.Tests
{
    [TestClass]
    public class SubscriptionTests : TestBase
    {
        /// <summary>
        /// PushBullets the get subscriptions test.
        /// </summary>
        [TestMethod]
        public void PushBulletGetSubscriptionsTest()
        {
            try
            {
                var subscriptions = Client.CurrentUsersSubscriptions();
                Assert.IsNotNull(subscriptions);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// PushBullets the get active subscriptions test.
        /// </summary>
        [TestMethod]
        public void PushBulletGetActiveSubscriptionsTest()
        {
            try
            {
                var activeSubscriptions = Client.CurrentUsersSubscriptions(true);
                Assert.IsNotNull(activeSubscriptions);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// PushBullets the get channel information test.
        /// </summary>
        [TestMethod]
        public void PushBulletGetChannelInformationTest()
        {
            try
            {
                var channelInfo = Client.GetChannelInformation("PushBulletSharp");
                Assert.IsNotNull(channelInfo);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// PushBullets the subscribe to channel test.
        /// </summary>
        [TestMethod]
        public void PushBulletSubscribeToChannelTest()
        {
            try
            {
                var response = Client.SubscribeToChannel("PushBulletSharp");
                Assert.IsNotNull(response);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// PushBullets the unsubscribe from channel test.
        /// </summary>
        [TestMethod]
        public void PushBulletUnsubscribeFromChannelTest()
        {
            try
            {
                var subscriptions = Client.CurrentUsersSubscriptions();
                var target = subscriptions.Subscriptions.Where(o => o.Channel.Tag == "pushbullet").FirstOrDefault();
                Assert.IsNotNull(target, "Could not find the target Channel");

                Assert.IsTrue(target.Active, "Target is not active. Cannot unsubscribe.");
                Client.UnsubscribeFromChannel(target.Iden);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
