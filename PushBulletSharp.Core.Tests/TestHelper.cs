using Microsoft.Extensions.Configuration;

namespace PushBulletSharp.Core.Tests
{
    public static class TestHelper
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            return config;
        }

        public static string GetConfig(string name)
        {
            var config = InitConfiguration();
            return config[name];
        }
    }
}
