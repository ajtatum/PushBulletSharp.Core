using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PushBulletSharp.Core.Tests
{
    public abstract class TestBase
    {
        public PushBulletClient Client { get; set; }
        public string ApiKey { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            var config = TestHelper.InitConfiguration();

            ApiKey = TestHelper.GetConfig("ApiKey");
            Client = new PushBulletClient(ApiKey);

            //Optional pass in your timezone
            //Client = new PushBulletClient(ApiKey, TimeZoneInfo.Local);

            //Or pass in a specific timezone
            //Client = new PushBulletClient(ApiKey, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }
    }
}
