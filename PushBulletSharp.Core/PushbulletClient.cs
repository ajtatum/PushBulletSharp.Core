using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushBulletSharp.Core.Filters;
using PushBulletSharp.Core.Models;
using PushBulletSharp.Core.Models.Requests;
using PushBulletSharp.Core.Models.Requests.Ephemerals;
using PushBulletSharp.Core.Models.Responses;
using PushBulletSharp.Core.Encryption;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PushBulletSharp.Core.Constants;
using PushBulletSharp.Core.Extensions;

namespace PushBulletSharp.Core
{
    /// <summary>
    /// PushBullet Client
    /// </summary>
    public class PushBulletClient
    {
        #region Constructors

        /// <summary>
        /// Creates a new PushBulletClient class.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="timeZoneInfo"></param>
        /// <exception cref="System.ArgumentNullException">accessToken</exception>
        public PushBulletClient(string accessToken, TimeZoneInfo timeZoneInfo = null)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            if (timeZoneInfo != null)
            {
                TimeZoneInfo = timeZoneInfo;
            }

            AccessToken = accessToken;
        }


        /// <summary>
        /// Creates a new PushBulletClient
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="encryptionPassword"></param>
        /// <param name="timeZoneInfo"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PushBulletClient(string accessToken, string encryptionPassword, TimeZoneInfo timeZoneInfo = null) : this(accessToken, timeZoneInfo)
        {
            if (string.IsNullOrWhiteSpace(encryptionPassword))
            {
                throw new ArgumentNullException(nameof(encryptionPassword));
            }

            SetEncryptionKey(encryptionPassword);
        }

        #endregion



        #region properties

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public string AccessToken { get; set; }


        private string _encryptionKey;
        /// <summary>
        /// Gets the Encryption Key
        /// </summary>
        public string EncryptionKey
        {
            get => _encryptionKey;
            set => SetEncryptionKey(value);
        }


        /// <summary>
        /// Gets the time zone information.
        /// </summary>
        /// <value>
        /// The time zone information.
        /// </value>
        internal TimeZoneInfo TimeZoneInfo { get; } = TimeZoneInfo.Utc;

        #endregion properties



        #region public methods

        #region User Information Methods

        /// <summary>
        /// Currents the users information.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<User> CurrentUsersInformation()
        {
            #region processing

            var result = await GetRequest<User>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.UsersUrls.Me)).ConfigureAwait(false);
            return result;

            #endregion processing
        }


        /// <summary>
        /// Currents the users devices.
        /// </summary>
        /// <param name="showActiveOnly">if set to <c>true</c> [show active only].</param>
        /// <returns></returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<UserDevices> CurrentUsersDevices(bool showActiveOnly = false)
        {
            #region pre-processing

            var additionalQuery = string.Empty;

            if (showActiveOnly)
            {
                additionalQuery = "?active=true";
            }

            #endregion end pre-processing

            #region processing

            var result = await GetRequest<UserDevices>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.DevicesUrls.Me, additionalQuery).Trim()).ConfigureAwait(false);
            return result;

            #endregion processing
        }

        #endregion User Information Methods


        #region Chats

        /// <summary>
        /// Currents the users chats.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<UserChats> CurrentUsersChats()
        {
            #region processing

            var basicResponse = await GetRequest<BasicChatsResponse>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.ChatsUrls.Chats).Trim()).ConfigureAwait(false);
            var result = ConvertBasicChatResponse(basicResponse);
            return result;

            #endregion processing
        }


        /// <summary>
        /// Creates the chat.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">create chat request</exception>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<Chat> CreateChat(CreateChatRequest request)
        {
            #region pre-processing

            if (request == null)
            {
                throw new ArgumentException("create chat request");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException(PushBulletConstants.CreateChatErrorMessages.ErrorEmailProperty);
            }

            #endregion pre-processing

            #region processing

            var basicResponse = await PostRequest<BasicChat>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.ChatsUrls.Chats), request).ConfigureAwait(false);
            var response = ConvertBasicChat(basicResponse);
            return response;

            #endregion processing
        }


        /// <summary>
        /// Updates the chat.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">update chat request</exception>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<Chat> UpdateChat(UpdateChatRequest request)
        {
            #region pre-processing

            if (request == null)
            {
                throw new ArgumentException("update chat request");
            }

            if (string.IsNullOrWhiteSpace(request.Iden))
            {
                throw new ArgumentException(PushBulletConstants.UpdateChatErrorMessages.ErrorIdenProperty);
            }

            #endregion pre-processing

            #region processing

            var basicResponse = await PostRequest<BasicChat>($"{PushBulletConstants.BaseUrl}{PushBulletConstants.ChatsUrls.Chats}/{request.Iden}", request).ConfigureAwait(false);
            var response = ConvertBasicChat(basicResponse);
            return response;

            #endregion processing
        }


        /// <summary>
        /// Deletes the chat.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentException">delete contact request</exception>
        public async Task<string> DeleteChat(DeleteChatRequest request)
        {
            #region pre-processing

            if (request == null)
            {
                throw new ArgumentException("delete chat request");
            }

            if (string.IsNullOrWhiteSpace(request.ChatIden))
            {
                throw new ArgumentException(PushBulletConstants.DeleteChatErrorMessages.ErrorIdenProperty);
            }

            #endregion pre-processing


            #region processing

            var result = await DeleteRequest($"{PushBulletConstants.BaseUrl}{PushBulletConstants.ChatsUrls.Chats}/{request.ChatIden}").ConfigureAwait(false);
            return result;

            #endregion processing
        }

        #endregion Chats


        #region Contacts Methods

        /// <summary>
        /// Currents the users contacts.
        /// </summary>
        /// <param name="showActiveOnly">if set to <c>true</c> [show active only].</param>
        /// <returns></returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<UserContacts> CurrentUsersContacts(bool showActiveOnly = false)
        {
            #region pre-processing

            var additionalQuery = string.Empty;

            if (showActiveOnly)
            {
                additionalQuery = "?active=true";
            }

            #endregion end pre-processing

            #region processing

            var result = await GetRequest<UserContacts>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.ContactsUrls.Contacts, additionalQuery).Trim()).ConfigureAwait(false);
            return result;

            #endregion processing
        }


        /// <summary>
        /// Creates the new contact.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">create contact request</exception>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<Contact> CreateNewContact(CreateContactRequest request)
        {
            #region pre-processing

            if (request == null)
            {
                throw new ArgumentException("create contact request");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException(PushBulletConstants.CreateContactErrorMessages.ErrorNameProperty);
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException(PushBulletConstants.CreateContactErrorMessages.ErrorEmailProperty);
            }

            #endregion pre-processing

            #region processing

            var response = await PostRequest<Contact>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.ContactsUrls.Contacts), request).ConfigureAwait(false);
            return response;

            #endregion processing
        }


        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">update contact request</exception>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<Contact> UpdateContact(UpdateContactRequest request)
        {
            #region pre-processing

            if (request == null)
            {
                throw new ArgumentException("update contact request");
            }

            if (string.IsNullOrWhiteSpace(request.ContactIden))
            {
                throw new ArgumentException(PushBulletConstants.UpdateContactErrorMessages.ErrorContactIdenProperty);
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException(PushBulletConstants.UpdateContactErrorMessages.ErrorNameProperty);
            }

            #endregion pre-processing


            #region processing

            var response = await PostRequest<Contact>(
                $"{PushBulletConstants.BaseUrl}{PushBulletConstants.ContactsUrls.Contacts}/{request.ContactIden}",
                request).ConfigureAwait(false);
            return response;

            #endregion processing
        }


        /// <summary>
        /// Deletes the contact.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentException">delete contact request</exception>
        public async Task<string> DeleteContact(DeleteContactRequest request)
        {
            #region pre-processing

            if (request == null)
            {
                throw new ArgumentException("delete contact request");
            }

            if (string.IsNullOrWhiteSpace(request.ContactIden))
            {
                throw new ArgumentException(PushBulletConstants.DeleteContactErrorMessages.ErrorContactIdenProperty);
            }

            #endregion pre-processing


            #region processing

            var result = await DeleteRequest($"{PushBulletConstants.BaseUrl}{PushBulletConstants.ContactsUrls.Contacts}/{request.ContactIden}").ConfigureAwait(false);
            return result;

            #endregion processing
        }

        #endregion Contacts Methods


        #region Channels Methods

        /// <summary>
        /// Subscribes to channel.
        /// </summary>
        /// <param name="channelTag">The channel tag.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">channel_tag</exception>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<Subscription> SubscribeToChannel(string channelTag)
        {
            #region pre-processing

            if (string.IsNullOrWhiteSpace(channelTag))
            {
                throw new ArgumentNullException(nameof(channelTag));
            }

            #endregion pre-processing


            #region processing

            var request = new ChannelSubscriptionRequest()
            {
                ChannelTag = channelTag
            };

            var basicResponse = await PostRequest<BasicSubscription>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.SubscriptionUrls.Subscriptions), request).ConfigureAwait(false);
            var result = ConvertFromBasicSubscription(basicResponse);
            return result;

            #endregion processing
        }


        /// <summary>
        /// Currents the users subscriptions.
        /// </summary>
        /// <param name="showActiveOnly">if set to <c>true</c> [show active only].</param>
        /// <returns></returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<UserSubscriptions> CurrentUsersSubscriptions(bool showActiveOnly = false)
        {
            #region pre-processing

            var additionalQuery = string.Empty;

            if (showActiveOnly)
            {
                additionalQuery = "?active=true";
            }

            #endregion end pre-processing

            #region processing

            var result = new UserSubscriptions();
            var basicResult = await GetRequest<BasicUserSubscriptions>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.SubscriptionUrls.Subscriptions, additionalQuery).Trim()).ConfigureAwait(false);
            foreach (var sub in basicResult.Subscriptions)
            {
                result.Subscriptions.Add(ConvertFromBasicSubscription(sub));
            }
            return result;

            #endregion processing
        }


        /// <summary>
        /// Converts from basic subscription.
        /// </summary>
        /// <param name="basicSubscription">The basic subscription.</param>
        /// <returns></returns>
        private Subscription ConvertFromBasicSubscription(BasicSubscription basicSubscription)
        {
            var result = new Subscription();
            result.Active = basicSubscription.Active;
            result.Channel = basicSubscription.Channel;
            result.Iden = basicSubscription.Iden;
            result.Created = TimeZoneInfo.ConvertTime(basicSubscription.Created.UnixTimeToDateTime(), TimeZoneInfo);
            result.Modified = TimeZoneInfo.ConvertTime(basicSubscription.Modified.UnixTimeToDateTime(), TimeZoneInfo);
            return result;
        }


        /// <summary>
        /// Unsubscribes from channel.
        /// </summary>
        /// <param name="channelIden">The channel iden.</param>
        /// <exception cref="System.ArgumentNullException">channelIden</exception>
        public async Task<string> UnsubscribeFromChannel(string channelIden)
        {
            #region pre-processing

            if (string.IsNullOrWhiteSpace(channelIden))
            {
                throw new ArgumentNullException(nameof(channelIden));
            }

            #endregion pre-processing


            #region processing

            var result = await DeleteRequest($"{PushBulletConstants.BaseUrl}{PushBulletConstants.SubscriptionUrls.Subscriptions}/{channelIden}").ConfigureAwait(false);
            return result;

            #endregion processing
        }


        /// <summary>
        /// Gets the channel information.
        /// </summary>
        /// <param name="channelTag">The channel tag.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">channel_tag</exception>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<Channel> GetChannelInformation(string channelTag)
        {
            #region pre-processing

            if (string.IsNullOrWhiteSpace(channelTag))
            {
                throw new ArgumentNullException(nameof(channelTag));
            }

            #endregion pre-processing


            #region processing

            var result = await GetRequest<Channel>($"{PushBulletConstants.BaseUrl}{PushBulletConstants.SubscriptionUrls.ChannelInfo}?tag={channelTag}").ConfigureAwait(false);
            return result;

            #endregion processing
        }

        #endregion Channels Methods


        #region Push Methods

        /// <summary>
        /// Pushes the note.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="ignoreEmptyFields">if set to <c>true</c> [ignore empty fields].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public async Task<PushResponse> PushNote(PushNoteRequest request, bool ignoreEmptyFields = false)
        {
            #region pre-processing

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Type))
            {
                throw new ArgumentNullException(PushBulletConstants.PushRequestErrorMessages.EmptyTypeProperty);
            }

            if (!ignoreEmptyFields)
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    throw new ArgumentNullException(PushBulletConstants.PushNoteRequestErrorMessages.EmptyTitleProperty);
                }

                if (string.IsNullOrWhiteSpace(request.Body))
                {
                    throw new ArgumentNullException(PushBulletConstants.PushNoteRequestErrorMessages.EmptyBodyProperty);
                }
            }

            #endregion pre-processing


            #region processing

            var result = await PostPushRequest<PushNoteRequest>(request).ConfigureAwait(false);
            return result;

            #endregion processing
        }


        /// <summary>
        /// Pushes the link.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="ignoreEmptyFields">if set to <c>true</c> [ignore empty fields].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.NullReferenceException"></exception>
        public async Task<PushResponse> PushLink(PushLinkRequest request, bool ignoreEmptyFields = false)
        {
            #region pre-processing

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Type))
            {
                throw new NullReferenceException(PushBulletConstants.PushRequestErrorMessages.EmptyTypeProperty);
            }

            if (!ignoreEmptyFields)
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    throw new NullReferenceException(PushBulletConstants.PushLinkErrorMessages.EmptyTitleProperty);
                }

                if (string.IsNullOrWhiteSpace(request.Url))
                {
                    throw new NullReferenceException(PushBulletConstants.PushLinkErrorMessages.EmptyUrlProperty);
                }

                //the body property is optional.
            }

            #endregion pre-processing


            #region processing

            var result = await PostPushRequest<PushLinkRequest>(request).ConfigureAwait(false);
            return result;

            #endregion processing
        }


        /// <summary>
        /// Pushes the file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">file request</exception>
        /// <exception cref="System.Exception"></exception>
        /// <exception cref="System.NullReferenceException"></exception>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<PushResponse> PushFile(PushFileRequest request)
        {
            #region pre-processing

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.FileName))
            {
                throw new NullReferenceException(PushBulletConstants.PushFileErrorMessages.EmptyFileNameProperty);
            }

            if (string.IsNullOrWhiteSpace(request.FileType))
            {
                throw new NullReferenceException(PushBulletConstants.PushFileErrorMessages.EmptyFileTypeProperty);
            }

            if (request.FileStream == null)
            {
                throw new NullReferenceException(PushBulletConstants.PushFileErrorMessages.EmptyFileStreamProperty);
            }

            if (!request.FileStream.CanRead)
            {
                throw new Exception(PushBulletConstants.PushFileErrorMessages.CantReadFileStreamProperty);
            }

            #endregion pre-processing


            #region processing

            var uploadRequestResponse = await PostRequest<FileUploadResponse>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.FileUrls.UploadRequest), request).ConfigureAwait(false);

            if (uploadRequestResponse.Data == null || string.IsNullOrWhiteSpace(uploadRequestResponse.FileUrl))
            {
                throw new Exception(PushBulletConstants.PushFileErrorMessages.ErrorMakingFileUploadRequest);
            }

            PushFileToAmazonAWS(request, uploadRequestResponse);
            request.FileUrl = uploadRequestResponse.FileUrl;
            return await PostPushRequest<PushFileRequest>(request).ConfigureAwait(false);

            #endregion processing
        }


        /// <summary>
        /// Gets the pushes.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<PushResponseContainer> GetPushes(PushResponseFilter filter)
        {
            #region pre-processing

            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var queryString = string.Empty;
            var queryStringList = new List<string>();

            if (!string.IsNullOrWhiteSpace(filter.Cursor))
            {
                var cursorQueryString = $"cursor={filter.Cursor}";
                queryStringList.Add(cursorQueryString);
            }
            else
            {
                if (filter.ModifiedDate != null)
                {
                    var modifiedDate = filter.ModifiedDate.DateTimeToUnixTime().ToString(System.Globalization.CultureInfo.InvariantCulture);
                    var modifiedDateQueryString = $"modified_after={modifiedDate}";
                    queryStringList.Add(modifiedDateQueryString);
                }

                if (filter.Active != null)
                {
                    var activeQueryString = $"active={((bool) filter.Active).ToString().ToLower()}";
                    queryStringList.Add(activeQueryString);
                }

                if (filter.Limit > 0)
                {
                    var limitQueryString = $"limit={filter.Limit}";
                    queryStringList.Add(limitQueryString);
                }
            }

            //Email filtering can be done on either cursor or regular queries
            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                var emailQueryString = $"email={filter.Email}";
                queryStringList.Add(emailQueryString);
            }

            //Join all of the query strings
            if (queryStringList.Any())
            {
                queryString = string.Concat("?", string.Join("&", queryStringList));
            }

            #endregion


            #region processing

            var results = new PushResponseContainer();
            var basicPushContainer = await GetRequest<BasicPushResponseContainer>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.PushesUrls.Pushes, queryString).Trim()).ConfigureAwait(false);
            var pushContainer = ConvertBasicPushResponseContainer(basicPushContainer);

            if (filter.IncludeTypes != null && filter.IncludeTypes.Any())
            {
                foreach (var type in filter.IncludeTypes)
                {
                    results.Pushes.AddRange(pushContainer.Pushes.Where(o => o.Type == type).ToList());
                }
                results.Pushes = results.Pushes.OrderByDescending(o => o.Created).ToList();
            }
            else
            {
                results = pushContainer;
            }

            return results;

            #endregion processing
        }


        /// <summary>
        /// Converts the basic push response container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private PushResponseContainer ConvertBasicPushResponseContainer(BasicPushResponseContainer container)
        {
            var result = new PushResponseContainer();
            foreach (var basicPush in container.Pushes)
            {
                result.Pushes.Add(ConvertBasicPushResponse(basicPush));
            }
            result.Cursor = container.Cursor;
            return result;
        }

        #endregion Push Methods


        #region OAuth Methods

        /// <summary>
        /// Requests the token.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">oauth token request</exception>
        /// <exception cref="System.Exception"></exception>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<OAuthTokenResponse> RequestToken(OAuthTokenRequest request)
        {
            try
            {
                #region pre-processing

                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                #endregion pre-processing


                #region processing

                var parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("grant_type", request.GrantType));
                parameters.Add(new KeyValuePair<string, string>("client_id", request.ClientId));
                parameters.Add(new KeyValuePair<string, string>("client_secret", request.ClientSecret));
                parameters.Add(new KeyValuePair<string, string>("code", request.Code));
                var jsonResult = await PostFormUrlEncodedContentRequest(string.Concat(PushBulletConstants.BaseUrlNonVersion, PushBulletConstants.OAuthUrls.OAuthToken), parameters).ConfigureAwait(false);
                var result = jsonResult.JsonToOjbect<OAuthTokenResponse>();
                return result;

                #endregion processing
            }
            catch (WebException ex)
            {
                var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                throw new Exception(string.Format(PushBulletConstants.OAuthErrorMessages.WebExceptionFormat, statusCode, ex.Message), ex);
            }
        }

        #endregion OAuth Methods


        #region Ephemerals

        /// <summary>
        /// Pushes the ephemeral.
        /// </summary>
        /// <param name="ephemeral">The ephemeral.</param>
        /// <param name="encrypt">if set to <c>true</c> [encrypt].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">ephemeral</exception>
        public async Task<string> PushEphemeral(IEphemeral ephemeral, bool encrypt = false)
        {
            #region pre-processing

            if (ephemeral == null)
            {
                throw new ArgumentNullException(nameof(ephemeral));
            }

            #endregion pre-processing


            #region processing

            if (encrypt)
            {
                var request = new EncryptedEphemeralRequest()
                {
                    Push = new EncryptedEphemeralMessage()
                    {
                        CipherText = EncryptionUtility.EncryptMessage(ephemeral.ToJson(), EncryptionKey)
                    }
                };
                return await PostEphemeralRequest(request).ConfigureAwait(false);
            }
            else
            {
                var request = new EphemeralRequest()
                {
                    Push = ephemeral
                };

                return await PostEphemeralRequest(request).ConfigureAwait(false);
            }

            #endregion processing
        }

        /// <summary>
        /// Pushes the ephemeral.
        /// </summary>
        /// <param name="jsonMessage">The json message.</param>
        /// <param name="encrypt">if set to <c>true</c> [encrypt].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">jsonMessage</exception>
        public async Task<string> PushEphemeral(string jsonMessage, bool encrypt = false)
        {
            #region pre-processing

            if (string.IsNullOrWhiteSpace(jsonMessage))
            {
                throw new ArgumentNullException(nameof(jsonMessage));
            }

            #endregion pre-processing


            #region processing

            if (encrypt)
            {
                var request = new EncryptedEphemeralRequest()
                {
                    Push = new EncryptedEphemeralMessage()
                    {
                        CipherText = EncryptionUtility.EncryptMessage(jsonMessage, EncryptionKey)
                    }
                };
                return await PostEphemeralRequest(request).ConfigureAwait(false);
            }
            else
            {
                var request = new StringEphemeralRequest()
                {
                    Push = jsonMessage
                };

                return await PostEphemeralRequest(request).ConfigureAwait(false);
            }

            #endregion processing
        }

        #endregion Ephemerals


        #region Web Socket Streaming

        /// <summary>
        /// Processes the stream response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Response data is null or empty!</exception>
        public Models.Responses.WebSocket.WebSocketResponse ProcessStreamResponse(string response)
        {
            #region pre-processing

            if (string.IsNullOrEmpty(response))
            {
                throw new Exception("Response data is null or empty!");
            }

            #endregion pre-processing


            #region processing

            var result = new Models.Responses.WebSocket.WebSocketResponse();

            var parsed = JObject.Parse(response);

            result.Subtype = parsed.SelectToken("subtype").Value<string>();
            result.Type = parsed.SelectToken("type").Value<string>();

            var push = parsed.SelectToken("push").ToString();

            var basicResponse = JsonConvert.DeserializeObject<Models.Responses.WebSocket.WebSocketPushResponse>(push);

            if (basicResponse.Encrypted)
            {
                var encryptedMessage = JsonConvert.DeserializeObject<EncryptedEphemeralMessage>(push);

                if (string.IsNullOrWhiteSpace(EncryptionKey))
                {
                    result.Push = encryptedMessage;
                }
                else
                {
                    var decryptedMessage = EncryptionUtility.DecryptMessage(encryptedMessage.CipherText, EncryptionKey);
                    var genericType = JsonConvert.DeserializeObject<Models.Responses.WebSocket.GenericPushTypeResponse>(decryptedMessage);

                    switch (genericType.Type)
                    {

                        default:
                            break;
                    }
                }
            }
            else
            {

            }

            return result;

            #endregion processing
        }

        #endregion Web Socket Streaming

        #endregion public methods



        #region private methods

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        private async Task<T> GetRequest<T>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add(PushBulletConstants.HeadersConstants.AuthorizationKey, string.Format(PushBulletConstants.HeadersConstants.AuthorizationValue, this.AccessToken));
            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request).ConfigureAwait(false);

                switch ((int) response.StatusCode)
                {
                    case (int) HttpStatusCode.OK:
                    {
                        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var output = result.JsonToOjbect<T>();
                        return output;
                    }
                    default:
                        HandleOtherStatusCodes(response.StatusCode);
                        throw new HttpRequestException(string.Format(PushBulletConstants.StatusCodeExceptions.Default, (int) response.StatusCode, response.StatusCode));
                }
            }
        }


        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="requestObject">The request object.</param>
        /// <returns></returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        private async Task<T> PostRequest<T>(string url, object requestObject)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add(PushBulletConstants.HeadersConstants.AuthorizationKey, string.Format(PushBulletConstants.HeadersConstants.AuthorizationValue, this.AccessToken));
            request.Content = new StringContent(requestObject.ToJson(), Encoding.UTF8, PushBulletConstants.MimeTypes.Json);

            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request).ConfigureAwait(false);

                switch ((int) response.StatusCode)
                {
                    case (int) HttpStatusCode.OK:
                    {
                        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var output = result.JsonToOjbect<T>();
                        return output;
                    }
                    default:
                        HandleOtherStatusCodes(response.StatusCode);
                        throw new HttpRequestException(string.Format(PushBulletConstants.StatusCodeExceptions.Default, (int) response.StatusCode, response.StatusCode));
                }
            }
        }


        /// <summary>
        /// Posts the form URL encoded content request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        private async Task<string> PostFormUrlEncodedContentRequest(string url, List<KeyValuePair<string, string>> parameters)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add(PushBulletConstants.HeadersConstants.AuthorizationKey, string.Format(PushBulletConstants.HeadersConstants.AuthorizationValue, this.AccessToken));
            //string postData = string.Join("&", parameters);
            request.Content = new FormUrlEncodedContent(parameters);

            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request).ConfigureAwait(false);

                switch ((int) response.StatusCode)
                {
                    case (int) HttpStatusCode.OK:
                    {
                        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return result;
                    }
                    default:
                        HandleOtherStatusCodes(response.StatusCode);
                        throw new HttpRequestException(string.Format(PushBulletConstants.StatusCodeExceptions.Default, (int) response.StatusCode, response.StatusCode));
                }
            }
        }


        /// <summary>
        /// Deletes the request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private async Task<string> DeleteRequest(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Add(PushBulletConstants.HeadersConstants.AuthorizationKey, string.Format(PushBulletConstants.HeadersConstants.AuthorizationValue, this.AccessToken));
            var client = new HttpClient();
            var response = await client.SendAsync(request).ConfigureAwait(false);

            switch ((int)response.StatusCode)
            {
                case (int)HttpStatusCode.OK:
                    {
                        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return result;
                    }
                default:
                    HandleOtherStatusCodes(response.StatusCode);
                    throw new HttpRequestException(string.Format(PushBulletConstants.StatusCodeExceptions.Default, (int)response.StatusCode, response.StatusCode));
            }
        }


        /// <summary>
        /// Handles the other status codes.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <exception cref="System.Net.Http.HttpRequestException">
        /// </exception>
        private void HandleOtherStatusCodes(HttpStatusCode statusCode)
        {
            switch ((int)statusCode)
            {
                case (int)HttpStatusCode.BadRequest:
                    throw new HttpRequestException(PushBulletConstants.StatusCodeExceptions.BadRequest);
                case (int)HttpStatusCode.Unauthorized:
                    throw new HttpRequestException(PushBulletConstants.StatusCodeExceptions.Unauthorized);
                case (int)HttpStatusCode.Forbidden:
                    throw new HttpRequestException(PushBulletConstants.StatusCodeExceptions.Forbidden);
                case (int)HttpStatusCode.NotFound:
                    throw new HttpRequestException(PushBulletConstants.StatusCodeExceptions.NotFound);
                case 429:
                    throw new HttpRequestException(PushBulletConstants.StatusCodeExceptions.BadRequest);
                case (int)HttpStatusCode.InternalServerError:
                case (int)HttpStatusCode.NotImplemented:
                case (int)HttpStatusCode.BadGateway:
                case (int)HttpStatusCode.ServiceUnavailable:
                case (int)HttpStatusCode.GatewayTimeout:
                case (int)HttpStatusCode.HttpVersionNotSupported:
                    throw new HttpRequestException(string.Format(PushBulletConstants.StatusCodeExceptions.FiveHundredXX, (int)statusCode, statusCode));
            }
        }


        /// <summary>
        /// Posts the ephemeral request.
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <returns></returns>
        private async Task<string> PostEphemeralRequest(EphemeralRequestBase requestObject)
        {
            var response = await PostRequest<string>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.EphemeralsUrls.Ephemerals), requestObject).ConfigureAwait(false);
            return response;
        }


        /// <summary>
        /// Posts the push request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestObject">The request object.</param>
        /// <returns></returns>
        private async Task<PushResponse> PostPushRequest<T>(T requestObject)
        {
            var basicResponse = await PostRequest<BasicPushResponse>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.PushesUrls.Pushes), requestObject).ConfigureAwait(false);
            var response = ConvertBasicPushResponse(basicResponse);
            return response;
        }


        /// <summary>
        /// Converts the basic push response.
        /// </summary>
        /// <param name="basicResponse">The basic response.</param>
        /// <returns></returns>
        private PushResponse ConvertBasicPushResponse(BasicPushResponse basicResponse)
        {
            var response = new PushResponse
            {
                Active = basicResponse.Active
            };
            if (basicResponse.Created != null)
            {
                response.Created = TimeZoneInfo.ConvertTime(basicResponse.Created.UnixTimeToDateTime(), TimeZoneInfo);
            }
            response.Dismissed = basicResponse.Dismissed;
            response.Direction = basicResponse.Direction;
            response.Iden = basicResponse.Iden;
            if (basicResponse.Modified != null)
            {
                response.Modified = TimeZoneInfo.ConvertTime(basicResponse.Modified.UnixTimeToDateTime(), TimeZoneInfo);
            }
            response.ReceiverEmail = basicResponse.ReceiverEmail;
            response.ReceiverEmailNormalized = basicResponse.ReceiverEmailNormalized;
            response.ReceiverIden = basicResponse.ReceiverIden;
            response.SenderEmail = basicResponse.SenderEmail;
            response.SenderEmailNormalized = basicResponse.SenderEmailNormalized;
            response.SenderIden = basicResponse.SenderIden;
            response.SenderName = basicResponse.SenderName;
            response.SourceDeviceIden = basicResponse.SourceDeviceIden;
            response.TargetDeviceIden = basicResponse.TargetDeviceIden;
            response.Type = ConvertPushResponseType(basicResponse.Type);
            response.ClientIden = basicResponse.ClientIden;
            response.Title = basicResponse.Title;
            response.Body = basicResponse.Body;
            response.Url = basicResponse.Url;
            response.FileName = basicResponse.FileName;
            response.FileType = basicResponse.FileType;
            response.FileUrl = basicResponse.FileUrl;
            response.ImageUrl = basicResponse.ImageUrl;
            response.Name = basicResponse.Name;
            return response;
        }


        /// <summary>
        /// Converts the basic chat response.
        /// </summary>
        /// <param name="basicResponse">The basic response.</param>
        /// <returns></returns>
        private UserChats ConvertBasicChatResponse(BasicChatsResponse basicResponse)
        {
            var response = new UserChats
            {
                Cursor = basicResponse.Cursor
            };

            foreach (var basicChat in basicResponse.Chats)
            {
                response.Chats.Add(ConvertBasicChat(basicChat));
            }

            return response;
        }

        private Chat ConvertBasicChat(BasicChat basicChat)
        {
            var chat = new Chat
            {
                Active = basicChat.Active
            };
            if (basicChat.Created != null)
            {
                chat.Created = TimeZoneInfo.ConvertTime(basicChat.Created.UnixTimeToDateTime(), TimeZoneInfo);
            }
            if (basicChat.Modified != null)
            {
                chat.Modified = TimeZoneInfo.ConvertTime(basicChat.Modified.UnixTimeToDateTime(), TimeZoneInfo);
            }
            chat.Iden = basicChat.Iden;
            chat.With = basicChat.With;

            return chat;
        }


        /// <summary>
        /// Converts the type of the push response.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private PushResponseType ConvertPushResponseType(string type)
        {
            switch (type)
            {
                case PushBulletConstants.TypeConstants.File:
                    return PushResponseType.File;
                case PushBulletConstants.TypeConstants.Link:
                    return PushResponseType.Link;
                case PushBulletConstants.TypeConstants.Note:
                default:
                    return PushResponseType.Note;
            }
        }


        /// <summary>
        /// Pushes the file to amazon aws.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="fileUploadResponse">The file upload response.</param>
        /// <exception cref="System.Exception"></exception>
        private async void PushFileToAmazonAWS(PushFileRequest request, FileUploadResponse fileUploadResponse)
        {
            StringContent awsaccesskeyidContent = null;
            StringContent aclContent = null;
            StringContent keyContent = null;
            StringContent signatureContent = null;
            StringContent policyContent = null;
            StringContent contentTypeContent = null;
            ByteArrayContent fileContent = null;

            try
            {
                using (var multiPartCont = new MultipartFormDataContent())
                {
                    awsaccesskeyidContent = CreateStringContentFromNameValue(FileUploadResponseData.Properties.AWSAccessKeyId, fileUploadResponse.Data.AWSAccessKeyId);
                    aclContent = CreateStringContentFromNameValue(FileUploadResponseData.Properties.Acl, fileUploadResponse.Data.Acl);
                    keyContent = CreateStringContentFromNameValue(FileUploadResponseData.Properties.Key, fileUploadResponse.Data.Key);
                    signatureContent = CreateStringContentFromNameValue(FileUploadResponseData.Properties.Signature, fileUploadResponse.Data.Signature);
                    policyContent = CreateStringContentFromNameValue(FileUploadResponseData.Properties.Policy, fileUploadResponse.Data.Policy);
                    contentTypeContent = CreateStringContentFromNameValue(PushBulletConstants.AmazonHeaders.ContentType, fileUploadResponse.FileType);

                    multiPartCont.Add(awsaccesskeyidContent);
                    multiPartCont.Add(aclContent);
                    multiPartCont.Add(keyContent);
                    multiPartCont.Add(signatureContent);
                    multiPartCont.Add(policyContent);
                    multiPartCont.Add(contentTypeContent);

                    using (var memoryStream = new MemoryStream())
                    {
                        await request.FileStream.CopyToAsync(memoryStream).ConfigureAwait(false);
                        fileContent = new ByteArrayContent(memoryStream.ToArray());
                    }

                    fileContent.Headers.Add(PushBulletConstants.AmazonHeaders.ContentType, PushBulletConstants.MimeTypes.OctetStream);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = $"\"{"file"}\"",
                        FileName = $"\"{request.FileName}\""
                    };

                    multiPartCont.Add(fileContent);

                    using (var httpClient = new HttpClient())
                    {
                        var httpRequest = httpClient.PostAsync(fileUploadResponse.UploadUrl, multiPartCont);
                        var httpResponse = await httpRequest.ConfigureAwait(false);

                        var xmlContentResponse = httpResponse.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(await xmlContentResponse.ConfigureAwait(false)))
                        {
                            throw new Exception(await xmlContentResponse.ConfigureAwait(false));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                awsaccesskeyidContent?.Dispose();
                aclContent?.Dispose();
                keyContent?.Dispose();
                signatureContent?.Dispose();
                policyContent?.Dispose();
                contentTypeContent?.Dispose();
                fileContent?.Dispose();
            }
        }


        /// <summary>
        /// Creates the string content from name value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private StringContent CreateStringContentFromNameValue(string name, string value)
        {
            var content = new StringContent(value);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = $"\"{name}\""
            };
            return content;
        }


        private async void SetEncryptionKey(string encryptionPassword)
        {
            var currentUser = await CurrentUsersInformation().ConfigureAwait(false);
            if (currentUser == null)
            {
                throw new Exception("Could not retrieve the current user information to create an encryption key.");
            }

            _encryptionKey = Encryption.EncryptionUtility.GenerateKey(currentUser.Iden, encryptionPassword);
        }

        #endregion private methods
    }
}