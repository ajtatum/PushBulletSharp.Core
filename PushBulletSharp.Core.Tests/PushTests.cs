using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushBulletSharp.Core.Filters;
using PushBulletSharp.Core.Models.Requests;
using PushBulletSharp.Core.Models.Responses;

namespace PushBulletSharp.Core.Tests
{
    [TestClass]
    public class PushTests : TestBase
    {
        /// <summary>
        /// Currents the user information test.
        /// </summary>
        [TestMethod]
        public async void CurrentUserInfoTest()
        {
            try
            {
                var response = await Client.CurrentUsersInformation();
                Assert.IsNotNull(response);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// Devices the test.
        /// </summary>
        [TestMethod]
        public async void DevicesTest()
        {
            try
            {
                var devices = await Client.CurrentUsersDevices();
                Assert.IsNotNull(devices);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// Actives the devices test.
        /// </summary>
        [TestMethod]
        public async void ActiveDevicesTest()
        {
            try
            {
                var devices = await Client.CurrentUsersDevices(true);
                Assert.IsNotNull(devices);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// PushBullets the push note test.
        /// </summary>
        [TestMethod]
        public async void PushBulletPushNoteTest()
        {
            try
            {
                var devices = await Client.CurrentUsersDevices();
                Assert.IsNotNull(devices);

                var device = devices.Devices.FirstOrDefault(o => o.Nickname == TestHelper.GetConfig("Device"));
                Assert.IsNotNull(device, "Could not find the device specified.");

                PushNoteRequest reqeust = new PushNoteRequest()
                {
                    DeviceIden = device.Iden,
                    Title = "hello world",
                    Body = "This is a test from my C# wrapper."
                };

                var response = await Client.PushNote(reqeust);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// PushBullets the push note by email test.
        /// This test shows how to push by using just an email address.
        /// This will result in all active devices receiving the push.
        /// </summary>
        [TestMethod]
        public async void PushBulletPushNoteByEmailTest()
        {
            try
            {
                var currentUserInformation = await Client.CurrentUsersInformation();
                Assert.IsNotNull(currentUserInformation);

                PushNoteRequest reqeust = new PushNoteRequest()
                {
                    Email = currentUserInformation.Email,
                    Title = "hello world via email",
                    Body = "This is a test from my C# wrapper."
                };

                var response = await Client.PushNote(reqeust);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// PushBullets the push link test.
        /// </summary>
        [TestMethod]
        public async void PushBulletPushLinkTest()
        {
            try
            {
                var devices = await Client.CurrentUsersDevices();
                Assert.IsNotNull(devices);

                var device = devices.Devices.FirstOrDefault(o => o.Nickname == TestHelper.GetConfig("Device"));
                Assert.IsNotNull(device, "Could not find the device specified.");

                PushLinkRequest reqeust = new PushLinkRequest()
                {
                    DeviceIden = device.Iden,
                    Title = "Google",
                    Url = "http://google.com/",
                    Body = "Search the internet."
                };

                var response = await Client.PushLink(reqeust);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// PushBullets the push file test.
        /// </summary>
        [TestMethod]
        public async void PushBulletPushFileTest()
        {
            try
            {
                var devices = await Client.CurrentUsersDevices();
                Assert.IsNotNull(devices);

                var device = devices.Devices.FirstOrDefault(o => o.Nickname == TestHelper.GetConfig("Device"));
                Assert.IsNotNull(device, "Could not find the device specified.");

                using (var fileStream = new FileStream(@"c:\daftpunk.png", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    PushFileRequest request = new PushFileRequest()
                    {
                        DeviceIden = device.Iden,
                        FileName = "daftpunk.png",
                        FileType = "image/png",
                        FileStream = fileStream,
                        Body = "Work It Harder\r\nMake It Better\r\nDo It Faster"
                    };

                    var response = await Client.PushFile(request);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async void GetPushesAllSince()
        {
            try
            {
                PushResponseFilter filter = new PushResponseFilter()
                {
                    ModifiedDate = new DateTime(2015,10,15,20,10,40,32),
                    Active = true
                };
                var results = await Client.GetPushes(filter);

                if (!string.IsNullOrWhiteSpace(results.Cursor))
                {
                    results = await Client.GetPushes(new PushResponseFilter(results.Cursor));
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async void GetPushesNotesAndFilesSince()
        {
            try
            {
                PushResponseFilter filter = new PushResponseFilter()
                {
                    ModifiedDate = new DateTime(2015, 3, 14),
                    IncludeTypes = new PushResponseType[] { PushResponseType.Note, PushResponseType.File }
                };
                var results = await Client.GetPushes(filter);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async void GetPushesAll()
        {
            try
            {
                var results = await Client.GetPushes(new PushResponseFilter());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async void GetPushesAllWithCursor()
        {
            try
            {
                PushResponseContainer results = await Client.GetPushes(new PushResponseFilter());

                while (!string.IsNullOrWhiteSpace(results.Cursor))
                {
                    results = await Client.GetPushes(new PushResponseFilter(results.Cursor));
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async void GatAllPushesByEmail()
        {
            try
            {
                var filter = new PushResponseFilter()
                {
                    Email = TestHelper.GetConfig("Email")
                };

                var results = await Client.GetPushes(filter);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async void GetPushesWithLimit()
        {
            try
            {
                var filter = new PushResponseFilter()
                {
                    Limit = 1
                };

                var results = await Client.GetPushes(filter);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async void GetPushesSinceLastPush()
        {
            try
            {
                // Push a first note
                PushNoteRequest request = new PushNoteRequest()
                {
                    Title = "Push one",
                    Body = "This is the first message."
                };
                var response = await Client.PushNote(request);

                // Get modified date of last push
                var results = await Client.GetPushes(new PushResponseFilter() { Limit = 1 });
                var lastModified = results.Pushes[0].Modified;

                Thread.Sleep(1000);

                // Push a second note
                var secondBody = "This is the second message.";
                request = new PushNoteRequest()
                {
                    Title = "Push two",
                    Body = secondBody
                };
                response = await Client.PushNote(request);

                // Get pushes since first one, + one millisecond because otherwise we can get back our previous received push...
                results = await Client.GetPushes(new PushResponseFilter() { ModifiedDate = lastModified.AddMilliseconds(1)});
                //should have only the second push
                Assert.AreEqual(1, results.Pushes.Count);
                Assert.AreEqual(secondBody, results.Pushes[0].Body);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async void GetPushesActive()
        {
            try
            {
                var filter = new PushResponseFilter()
                {
                    Active = true
                };

                var results = await Client.GetPushes(filter);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}