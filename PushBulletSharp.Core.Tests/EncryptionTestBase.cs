using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PushBulletSharp.Core.Tests
{
    public abstract class EncryptionTestBase
    {
        public PushBulletClient Client { get; set; }
        public string ApiKey { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ApiKey = "--YOURKEYGOESHERE--";
            Client = new PushBulletClient(ApiKey, "--YOUR-ENCRYPTION-PASSWORD--", TimeZoneInfo.Local);

            //Optional pass in your timezone
            //Client = new PushBulletClient(ApiKey, TimeZoneInfo.Local);

            //Or pass in a specific timezone
            //Client = new PushBulletClient(ApiKey, "password", TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }
    }
}