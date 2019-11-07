﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushBulletSharp.Core.Models.Requests;

namespace PushBulletSharp.Core.Tests
{
    [TestClass]
    public class ChatsTest : TestBase
    {
        /// <summary>
        /// Lists the chats test.
        /// </summary>
        [TestMethod]
        public void ListChatsTest()
        {
            try
            {
                var chats = Client.CurrentUsersChats();
                Assert.IsNotNull(chats);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Creates the chat test.
        /// </summary>
        [TestMethod]
        public void CreateChatTest()
        {
            try
            {
                CreateChatRequest request = new CreateChatRequest()
                {
                    Email = "some.person@aninternetwebsite.com"
                };
                var response = Client.CreateChat(request);
                Assert.IsNotNull(response);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Updates the chat mute true test.
        /// </summary>
        [TestMethod]
        public async void UpdateChatMuteTrueTest()
        {
            try
            {
                var chats = await Client.CurrentUsersChats();

                if (chats != null && chats.Chats.Count > 0)
                {
                    var firstChat = chats.Chats.FirstOrDefault();

                    if (firstChat != null)
                    {
                        UpdateChatRequest request = new UpdateChatRequest()
                        {
                            Iden = firstChat.Iden,
                            Muted = true
                        };

                        var result = Client.UpdateChat(request);
                        Assert.IsNotNull(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Updates the chat mute false test.
        /// </summary>
        [TestMethod]
        public async void UpdateChatMuteFalseTest()
        {
            try
            {
                var chats = await Client.CurrentUsersChats();

                if (chats != null && chats.Chats.Count > 0)
                {
                    var firstChat = chats.Chats.FirstOrDefault();

                    if (firstChat != null)
                    {
                        UpdateChatRequest request = new UpdateChatRequest()
                        {
                            Iden = firstChat.Iden,
                            Muted = false
                        };

                        var result = Client.UpdateChat(request);
                        Assert.IsNotNull(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the chat test.
        /// </summary>
        [TestMethod]
        public async void DeleteChatTest()
        {
            try
            {
                var chats = await Client.CurrentUsersChats();

                if (chats != null && chats.Chats.Count > 0)
                {
                    var firstChat = chats.Chats.FirstOrDefault();

                    if (firstChat != null)
                    {
                        DeleteChatRequest request = new DeleteChatRequest()
                        {
                            ChatIden = firstChat.Iden
                        };

                        await Client.DeleteChat(request);
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}