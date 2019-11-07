using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushBulletSharp.Core.Models.Requests;

namespace PushBulletSharp.Core.Tests
{
    [TestClass]
    public class ContactsTests : TestBase
    {
        /// <summary>
        /// PushBullets the get contacts test.
        /// </summary>
        [TestMethod]
        public void PushBulletGetContactsTest()
        {
            try
            {
                var contacts = Client.CurrentUsersContacts();
                Assert.IsNotNull(contacts);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// PushBullets the get active contacts test.
        /// </summary>
        [TestMethod]
        public void PushBulletGetActiveContactsTest()
        {
            try
            {
                var contacts = Client.CurrentUsersContacts(true);
                Assert.IsNotNull(contacts);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// PushBullets the create contact test.
        /// </summary>
        [TestMethod]
        public void PushBulletCreateContactTest()
        {
            try
            {
                CreateContactRequest request = new CreateContactRequest()
                {
                    Name = "Some Person",
                    Email = "some.person@aninternetwebsite.com"
                };

                var result = Client.CreateNewContact(request);
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// PushBullets the update contact test.
        /// </summary>
        [TestMethod]
        public void PushBulletUpdateContactTest()
        {
            try
            {
                var contacts = Client.CurrentUsersContacts();
                Assert.IsNotNull(contacts);

                var contact = contacts.Contacts.FirstOrDefault(o => o.Email == "some.person@aninternetwebsite.com");
                Assert.IsNotNull(contact);

                UpdateContactRequest request = new UpdateContactRequest()
                {
                    Name = "Not A Real Person",
                    ContactIden = contact.Iden
                };

                var result = Client.UpdateContact(request);
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        /// <summary>
        /// PushBullets the delete contact test.
        /// </summary>
        [TestMethod]
        public void PushBulletDeleteContactTest()
        {
            try
            {
                var contacts = Client.CurrentUsersContacts();
                Assert.IsNotNull(contacts);

                var contact = contacts.Contacts.Where(o => o.Email == "some.person@aninternetwebsite.com").FirstOrDefault();
                Assert.IsNotNull(contact);

                DeleteContactRequest request = new DeleteContactRequest()
                {
                    ContactIden = contact.Iden
                };

                Client.DeleteContact(request);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}