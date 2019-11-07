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
    public class PushBulletClient
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PushBulletManager"/> class.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <exception cref="System.ArgumentNullException">accessToken</exception>
        public PushBulletClient(string accessToken, TimeZoneInfo timeZoneInfo = null)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            if (timeZoneInfo != null)
            {
                _timeZoneInfo = timeZoneInfo;
            }

            _accessToken = accessToken;
        }


        public PushBulletClient(string accessToken, string encryptionPassword, TimeZoneInfo timeZoneInfo = null) : this(accessToken, timeZoneInfo)
        {
            if (string.IsNullOrWhiteSpace(encryptionPassword))
            {
                throw new ArgumentNullException("encryptionPassword");
            }

            SetEncryptionKey(encryptionPassword);
        }

        #endregion



        #region properties

        private string _accessToken;
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public string AccessToken
        {
            get
            {
                return _accessToken;
            }
            set
            {
                _accessToken = value;
            }
        }


        private string _encryptionKey;
        public string EncryptionKey
        {
            get
            {
                return _encryptionKey;
            }
            set
            {
                SetEncryptionKey(value);
            }
        }


        private TimeZoneInfo _timeZoneInfo = TimeZoneInfo.Utc;
        /// <summary>
        /// Gets the time zone information.
        /// </summary>
        /// <value>
        /// The time zone information.
        /// </value>
        internal TimeZoneInfo TimeZoneInfo
        {
            get
            {
                return _timeZoneInfo;
            }
        }

        #endregion properties



        #region public methods

        #region User Information Methods

        /// <summary>
        /// Currents the users information.
        /// </summary>
        /// <returns></returns>
        public User CurrentUsersInformation()
        {
            try
            {
                #region processing

                User result = GetRequest<User>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.UsersUrls.Me));
                return result;

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Currents the users devices.
        /// </summary>
        /// <param name="showActiveOnly">if set to <c>true</c> [show active only].</param>
        /// <returns></returns>
        public UserDevices CurrentUsersDevices(bool showActiveOnly = false)
        {
            try
            {
                #region pre-processing

                string additionalQuery = string.Empty;

                if (showActiveOnly)
                {
                    additionalQuery = "?active=true";
                }

                #endregion end pre-processing

                #region processing

                UserDevices result = GetRequest<UserDevices>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.DevicesUrls.Me, additionalQuery).Trim());
                return result;

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion User Information Methods


        #region Chats

        /// <summary>
        /// Currents the users chats.
        /// </summary>
        /// <returns></returns>
        public UserChats CurrentUsersChats()
        {
            try
            {
                #region processing

                var basicResponse = GetRequest<BasicChatsResponse>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.ChatsUrls.Chats).Trim());
                UserChats result = ConvertBasicChatResponse(basicResponse);
                return result;

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Creates the chat.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">create chat request</exception>
        public Chat CreateChat(CreateChatRequest request)
        {
            try
            {
                #region pre-processing

                if (request == null)
                {
                    throw new ArgumentException("create chat request");
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    throw new Exception(PushBulletConstants.CreateChatErrorMessages.ErrorEmailProperty);
                }

                #endregion pre-processing

                #region processing

                BasicChat basicResponse = PostRequest<BasicChat>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.ChatsUrls.Chats), request);
                Chat response = ConvertBasicChat(basicResponse);
                return response;

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Updates the chat.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">update chat request</exception>
        /// <exception cref="System.Exception"></exception>
        public Chat UpdateChat(UpdateChatRequest request)
        {
            try
            {
                #region pre-processing

                if (request == null)
                {
                    throw new ArgumentException("update chat request");
                }

                if (string.IsNullOrWhiteSpace(request.Iden))
                {
                    throw new Exception(PushBulletConstants.UpdateChatErrorMessages.ErrorIdenProperty);
                }

                #endregion pre-processing

                #region processing

                BasicChat basicResponse = PostRequest<BasicChat>(string.Format("{0}{1}/{2}", PushBulletConstants.BaseUrl, PushBulletConstants.ChatsUrls.Chats, request.Iden), request);
                Chat response = ConvertBasicChat(basicResponse);
                return response;

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Deletes the chat.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentException">delete contact request</exception>
        /// <exception cref="System.Exception"></exception>
        public void DeleteChat(DeleteChatRequest request)
        {
            try
            {
                #region pre-processing

                if (request == null)
                {
                    throw new ArgumentException("delete chat request");
                }

                if (string.IsNullOrWhiteSpace(request.ChatIden))
                {
                    throw new Exception(PushBulletConstants.DeleteChatErrorMessages.ErrorIdenProperty);
                }

                #endregion pre-processing


                #region processing

                string jsonResult = DeleteRequest(string.Format("{0}{1}/{2}", PushBulletConstants.BaseUrl, PushBulletConstants.ChatsUrls.Chats, request.ChatIden));

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Chats


        #region Contacts Methods

        /// <summary>
        /// Currents the users contacts.
        /// </summary>
        /// <param name="showActiveOnly">if set to <c>true</c> [show active only].</param>
        /// <returns></returns>
        public UserContacts CurrentUsersContacts(bool showActiveOnly = false)
        {
            try
            {
                #region pre-processing

                string additionalQuery = string.Empty;

                if (showActiveOnly)
                {
                    additionalQuery = "?active=true";
                }

                #endregion end pre-processing

                #region processing

                UserContacts result = GetRequest<UserContacts>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.ContactsUrls.Contacts, additionalQuery).Trim());
                return result;

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Creates the new contact.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">create contact request</exception>
        /// <exception cref="System.Exception">
        /// </exception>
        public Contact CreateNewContact(CreateContactRequest request)
        {
            try
            {
                #region pre-processing

                if (request == null)
                {
                    throw new ArgumentException("create contact request");
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    throw new Exception(PushBulletConstants.CreateContactErrorMessages.ErrorNameProperty);
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    throw new Exception(PushBulletConstants.CreateContactErrorMessages.ErrorEmailProperty);
                }

                #endregion pre-processing

                #region processing

                Contact response = PostRequest<Contact>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.ContactsUrls.Contacts), request);
                return response;

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">update contact request</exception>
        /// <exception cref="System.Exception">
        /// </exception>
        public Contact UpdateContact(UpdateContactRequest request)
        {
            try
            {
                #region pre-processing

                if (request == null)
                {
                    throw new ArgumentException("update contact request");
                }

                if (string.IsNullOrWhiteSpace(request.ContactIden))
                {
                    throw new Exception(PushBulletConstants.UpdateContactErrorMessages.ErrorContactIdenProperty);
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    throw new Exception(PushBulletConstants.UpdateContactErrorMessages.ErrorNameProperty);
                }

                #endregion pre-processing


                #region processing

                Contact response = PostRequest<Contact>(
                    string.Format("{0}{1}/{2}",
                        PushBulletConstants.BaseUrl,
                        PushBulletConstants.ContactsUrls.Contacts,
                        request.ContactIden),
                    request);
                return response;

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Deletes the contact.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentException">delete contact request</exception>
        /// <exception cref="System.Exception"></exception>
        public void DeleteContact(DeleteContactRequest request)
        {
            try
            {
                #region pre-processing

                if (request == null)
                {
                    throw new ArgumentException("delete contact request");
                }

                if (string.IsNullOrWhiteSpace(request.ContactIden))
                {
                    throw new Exception(PushBulletConstants.DeleteContactErrorMessages.ErrorContactIdenProperty);
                }

                #endregion pre-processing


                #region processing

                string jsonResult = DeleteRequest(string.Format("{0}{1}/{2}", PushBulletConstants.BaseUrl, PushBulletConstants.ContactsUrls.Contacts, request.ContactIden));

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Contacts Methods


        #region Channels Methods

        /// <summary>
        /// Subscribes to channel.
        /// </summary>
        /// <param name="channelTag">The channel tag.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">channel_tag</exception>
        public Subscription SubscribeToChannel(string channelTag)
        {
            #region pre-processing

            if (string.IsNullOrWhiteSpace(channelTag))
            {
                throw new ArgumentNullException("channel_tag");
            }

            #endregion pre-processing


            #region processing

            ChannelSubscriptionRequest request = new ChannelSubscriptionRequest()
            {
                ChannelTag = channelTag
            };

            BasicSubscription basicResponse = PostRequest<BasicSubscription>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.SubscriptionUrls.Subscriptions), request);
            Subscription result = ConvertFromBasicSubscription(basicResponse);
            return result;

            #endregion processing
        }


        /// <summary>
        /// Currents the users subscriptions.
        /// </summary>
        /// <param name="showActiveOnly">if set to <c>true</c> [show active only].</param>
        /// <returns></returns>
        public UserSubscriptions CurrentUsersSubscriptions(bool showActiveOnly = false)
        {
            try
            {
                #region pre-processing

                string additionalQuery = string.Empty;

                if (showActiveOnly)
                {
                    additionalQuery = "?active=true";
                }

                #endregion end pre-processing

                #region processing

                UserSubscriptions result = new UserSubscriptions();
                var basicResult = GetRequest<BasicUserSubscriptions>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.SubscriptionUrls.Subscriptions, additionalQuery).Trim());
                foreach (var sub in basicResult.Subscriptions)
                {
                    result.Subscriptions.Add(ConvertFromBasicSubscription(sub));
                }
                return result;

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Converts from basic subscription.
        /// </summary>
        /// <param name="basicSubscription">The basic subscription.</param>
        /// <returns></returns>
        private Subscription ConvertFromBasicSubscription(BasicSubscription basicSubscription)
        {
            Subscription result = new Subscription();
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
        /// <exception cref="System.ArgumentNullException">channel_iden</exception>
        public void UnsubscribeFromChannel(string channelIden)
        {
            #region pre-processing

            if (string.IsNullOrWhiteSpace(channelIden))
            {
                throw new ArgumentNullException("channel_iden");
            }

            #endregion pre-processing


            #region processing

            string jsonResult = DeleteRequest(string.Format("{0}{1}/{2}", PushBulletConstants.BaseUrl, PushBulletConstants.SubscriptionUrls.Subscriptions, channelIden));

            #endregion processing
        }


        /// <summary>
        /// Gets the channel information.
        /// </summary>
        /// <param name="channelTag">The channel tag.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">channel_tag</exception>
        public Channel GetChannelInformation(string channelTag)
        {
            #region pre-processing

            if (string.IsNullOrWhiteSpace(channelTag))
            {
                throw new ArgumentNullException("channel_tag");
            }

            #endregion pre-processing


            #region processing

            Channel result = GetRequest<Channel>(string.Format("{0}{1}?tag={2}", PushBulletConstants.BaseUrl, PushBulletConstants.SubscriptionUrls.ChannelInfo, channelTag));
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
        /// <exception cref="System.ArgumentNullException">note request</exception>
        /// <exception cref="System.Exception">
        /// </exception>
        public PushResponse PushNote(PushNoteRequest request, bool ignoreEmptyFields = false)
        {
            try
            {
                #region pre-processing

                if (request == null)
                {
                    throw new ArgumentNullException("note request");
                }

                if (string.IsNullOrWhiteSpace(request.Type))
                {
                    throw new Exception(PushBulletConstants.PushRequestErrorMessages.EmptyTypeProperty);
                }

                if (!ignoreEmptyFields)
                {
                    if (string.IsNullOrWhiteSpace(request.Title))
                    {
                        throw new Exception(PushBulletConstants.PushNoteRequestErrorMessages.EmptyTitleProperty);
                    }

                    if (string.IsNullOrWhiteSpace(request.Body))
                    {
                        throw new Exception(PushBulletConstants.PushNoteRequestErrorMessages.EmptyBodyProperty);
                    }
                }

                #endregion pre-processing


                #region processing

                return PostPushRequest<PushNoteRequest>(request);

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Pushes the link.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="ignoreEmptyFields">if set to <c>true</c> [ignore empty fields].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">link request</exception>
        /// <exception cref="System.Exception">
        /// </exception>
        public PushResponse PushLink(PushLinkRequest request, bool ignoreEmptyFields = false)
        {
            try
            {
                #region pre-processing

                if (request == null)
                {
                    throw new ArgumentNullException("link request");
                }

                if (string.IsNullOrWhiteSpace(request.Type))
                {
                    throw new Exception(PushBulletConstants.PushRequestErrorMessages.EmptyTypeProperty);
                }

                if (!ignoreEmptyFields)
                {
                    if (string.IsNullOrWhiteSpace(request.Title))
                    {
                        throw new Exception(PushBulletConstants.PushLinkErrorMessages.EmptyTitleProperty);
                    }

                    if (string.IsNullOrWhiteSpace(request.Url))
                    {
                        throw new Exception(PushBulletConstants.PushLinkErrorMessages.EmptyUrlProperty);
                    }

                    //the body property is optional.
                }

                #endregion pre-processing


                #region processing

                return PostPushRequest<PushLinkRequest>(request);

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Pushes the file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">file request</exception>
        /// <exception cref="System.Exception">
        /// </exception>
        public PushResponse PushFile(PushFileRequest request)
        {
            try
            {
                #region pre-processing

                if (request == null)
                {
                    throw new ArgumentNullException("file request");
                }

                if (string.IsNullOrWhiteSpace(request.FileName))
                {
                    throw new Exception(PushBulletConstants.PushFileErrorMessages.EmptyFileNameProperty);
                }

                if (string.IsNullOrWhiteSpace(request.FileType))
                {
                    throw new Exception(PushBulletConstants.PushFileErrorMessages.EmptyFileTypeProperty);
                }

                if (request.FileStream == null)
                {
                    throw new Exception(PushBulletConstants.PushFileErrorMessages.EmptyFileStreamProperty);
                }

                if (!request.FileStream.CanRead)
                {
                    throw new Exception(PushBulletConstants.PushFileErrorMessages.CantReadFileStreamProperty);
                }

                #endregion pre-processing


                #region processing

                FileUploadResponse uploadRequestResponse = PostRequest<FileUploadResponse>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.FileUrls.UploadRequest), request);

                if (uploadRequestResponse.Data == null || string.IsNullOrWhiteSpace(uploadRequestResponse.FileUrl))
                {
                    throw new Exception(PushBulletConstants.PushFileErrorMessages.ErrorMakingFileUploadRequest);
                }

                PushFileToAmazonAWS(request, uploadRequestResponse);
                request.FileUrl = uploadRequestResponse.FileUrl;
                return PostPushRequest<PushFileRequest>(request);

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the pushes.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// filter
        /// or
        /// filter
        /// </exception>
        /// <exception cref="System.Exception">Connect issue.</exception>
        public PushResponseContainer GetPushes(PushResponseFilter filter)
        {
            try
            {
                #region pre-processing

                if (filter == null)
                {
                    throw new ArgumentNullException("filter");
                }

                string queryString = string.Empty;
                List<string> queryStringList = new List<string>();

                if (!string.IsNullOrWhiteSpace(filter.Cursor))
                {
                    string cursorQueryString = string.Format("cursor={0}", filter.Cursor);
                    queryStringList.Add(cursorQueryString);
                }
                else
                {
                    if (filter.ModifiedDate != null)
                    {
                        string modifiedDate = filter.ModifiedDate.DateTimeToUnixTime().ToString(System.Globalization.CultureInfo.InvariantCulture);
                        string modifiedDateQueryString = string.Format("modified_after={0}", modifiedDate);
                        queryStringList.Add(modifiedDateQueryString);
                    }

                    if (filter.Active != null)
                    {
                        string activeQueryString = string.Format("active={0}", ((bool)filter.Active).ToString().ToLower());
                        queryStringList.Add(activeQueryString);
                    }

                    if (filter.Limit > 0)
                    {
                        string limitQueryString = string.Format("limit={0}", filter.Limit);
                        queryStringList.Add(limitQueryString);
                    }
                }

                //Email filtering can be done on either cursor or regular queries
                if (!string.IsNullOrWhiteSpace(filter.Email))
                {
                    string emailQueryString = string.Format("email={0}", filter.Email);
                    queryStringList.Add(emailQueryString);
                }

                //Join all of the query strings
                if (queryStringList.Count() > 0)
                {
                    queryString = string.Concat("?", string.Join("&", queryStringList));
                }

                #endregion


                #region processing

                PushResponseContainer results = new PushResponseContainer();
                BasicPushResponseContainer basicPushContainer = GetRequest<BasicPushResponseContainer>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.PushesUrls.Pushes, queryString).Trim());
                PushResponseContainer pushContainer = ConvertBasicPushResponseContainer(basicPushContainer);

                if (filter.IncludeTypes != null && filter.IncludeTypes.Count() > 0)
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
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Converts the basic push response container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private PushResponseContainer ConvertBasicPushResponseContainer(BasicPushResponseContainer container)
        {
            PushResponseContainer result = new PushResponseContainer();
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
        public OAuthTokenResponse RequestToken(OAuthTokenRequest request)
        {
            try
            {
                #region pre-processing

                if (request == null)
                {
                    throw new ArgumentNullException("oauth token request");
                }

                #endregion pre-processing


                #region processing

                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("grant_type", request.GrantType));
                parameters.Add(new KeyValuePair<string, string>("client_id", request.ClientId));
                parameters.Add(new KeyValuePair<string, string>("client_secret", request.ClientSecret));
                parameters.Add(new KeyValuePair<string, string>("code", request.Code));
                string jsonResult = PostFormUrlEncodedContentRequest(string.Concat(PushBulletConstants.BaseUrlNonVersion, PushBulletConstants.OAuthUrls.OAuthToken), parameters);
                var result = jsonResult.JsonToOjbect<OAuthTokenResponse>();
                return result;

                #endregion processing
            }
            catch (WebException ex)
            {
                var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                throw new Exception(string.Format(PushBulletConstants.OAuthErrorMessages.WebExceptionFormat, statusCode, ex.Message), ex);
            }
            catch (Exception)
            {
                throw;
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
        public string PushEphemeral(IEphemeral ephemeral, bool encrypt = false)
        {
            try
            {
                #region pre-processing

                if (ephemeral == null)
                {
                    throw new ArgumentNullException("ephemeral");
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
                    return PostEphemeralRequest(request);
                }
                else
                {
                    var request = new EphemeralRequest()
                    {
                        Push = ephemeral
                    };

                    return PostEphemeralRequest(request);
                }

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Pushes the ephemeral.
        /// </summary>
        /// <param name="jsonMessage">The json message.</param>
        /// <param name="encrypt">if set to <c>true</c> [encrypt].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">jsonMessage</exception>
        public string PushEphemeral(string jsonMessage, bool encrypt = false)
        {
            try
            {
                #region pre-processing

                if (string.IsNullOrWhiteSpace(jsonMessage))
                {
                    throw new ArgumentNullException("jsonMessage");
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
                    return PostEphemeralRequest(request);
                }
                else
                {
                    var request = new StringEphemeralRequest()
                    {
                        Push = jsonMessage
                    };

                    return PostEphemeralRequest(request);
                }

                #endregion processing
            }
            catch (Exception)
            {
                throw;
            }
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

            Models.Responses.WebSocket.WebSocketResponse result = new Models.Responses.WebSocket.WebSocketResponse();

            var parsed = JObject.Parse(response);

            result.Subtype = parsed.SelectToken("subtype").Value<string>();
            result.Type = parsed.SelectToken("type").Value<string>();

            string push = parsed.SelectToken("push").ToString();

            var basicResponse = JsonConvert.DeserializeObject<Models.Responses.WebSocket.WebSocketPushResponse>(push);

            if (basicResponse.Encrypted)
            {
                EncryptedEphemeralMessage encryptedMessage = JsonConvert.DeserializeObject<EncryptedEphemeralMessage>(push);

                if (string.IsNullOrWhiteSpace(EncryptionKey))
                {
                    result.Push = encryptedMessage;
                }
                else
                {
                    string decryptedMessage = EncryptionUtility.DecryptMessage(encryptedMessage.CipherText, EncryptionKey);
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
        private T GetRequest<T>(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add(PushBulletConstants.HeadersConstants.AuthorizationKey, string.Format(PushBulletConstants.HeadersConstants.AuthorizationValue, this.AccessToken));
            HttpClient client = new HttpClient();
            var response = client.SendAsync(request).Result;

            switch ((int)response.StatusCode)
            {
                case (int)HttpStatusCode.OK:
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var output = result.JsonToOjbect<T>();
                        return output;
                    }
                default:
                    HandleOtherStatusCodes(response.StatusCode);
                    throw new HttpRequestException(string.Format(PushBulletConstants.StatusCodeExceptions.Default, (int)response.StatusCode, response.StatusCode));
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
        private T PostRequest<T>(string url, object requestObject)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add(PushBulletConstants.HeadersConstants.AuthorizationKey, string.Format(PushBulletConstants.HeadersConstants.AuthorizationValue, this.AccessToken));
            request.Content = new StringContent(requestObject.ToJson(), Encoding.UTF8, PushBulletConstants.MimeTypes.Json);

            HttpClient client = new HttpClient();
            var response = client.SendAsync(request).Result;

            switch ((int)response.StatusCode)
            {
                case (int)HttpStatusCode.OK:
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var output = result.JsonToOjbect<T>();
                        return output;
                    }
                default:
                    HandleOtherStatusCodes(response.StatusCode);
                    throw new HttpRequestException(string.Format(PushBulletConstants.StatusCodeExceptions.Default, (int)response.StatusCode, response.StatusCode));
            }
        }


        /// <summary>
        /// Posts the form URL encoded content request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        private string PostFormUrlEncodedContentRequest(string url, List<KeyValuePair<string, string>> parameters)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add(PushBulletConstants.HeadersConstants.AuthorizationKey, string.Format(PushBulletConstants.HeadersConstants.AuthorizationValue, this.AccessToken));
                //string postData = string.Join("&", parameters);
                request.Content = new FormUrlEncodedContent(parameters);

                HttpClient client = new HttpClient();
                var response = client.SendAsync(request).Result;

                switch ((int)response.StatusCode)
                {
                    case (int)HttpStatusCode.OK:
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            return result;
                        }
                    default:
                        HandleOtherStatusCodes(response.StatusCode);
                        throw new HttpRequestException(string.Format(PushBulletConstants.StatusCodeExceptions.Default, (int)response.StatusCode, response.StatusCode));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Deletes the request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private string DeleteRequest(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Add(PushBulletConstants.HeadersConstants.AuthorizationKey, string.Format(PushBulletConstants.HeadersConstants.AuthorizationValue, this.AccessToken));
            HttpClient client = new HttpClient();
            var response = client.SendAsync(request).Result;

            switch ((int)response.StatusCode)
            {
                case (int)HttpStatusCode.OK:
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
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
        private string PostEphemeralRequest(EphemeralRequestBase requestObject)
        {
            var response = PostRequest<string>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.EphemeralsUrls.Ephemerals), requestObject);
            return response;
        }


        /// <summary>
        /// Posts the push request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestObject">The request object.</param>
        /// <returns></returns>
        private PushResponse PostPushRequest<T>(T requestObject)
        {
            var basicResponse = PostRequest<BasicPushResponse>(string.Concat(PushBulletConstants.BaseUrl, PushBulletConstants.PushesUrls.Pushes), requestObject);
            PushResponse response = ConvertBasicPushResponse(basicResponse);
            return response;
        }


        /// <summary>
        /// Converts the basic push response.
        /// </summary>
        /// <param name="basicResponse">The basic response.</param>
        /// <returns></returns>
        private PushResponse ConvertBasicPushResponse(BasicPushResponse basicResponse)
        {
            PushResponse response = new PushResponse();
            response.Active = basicResponse.Active;
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
            UserChats response = new UserChats();
            response.Cursor = basicResponse.Cursor;

            foreach (var basicChat in basicResponse.Chats)
            {
                response.Chats.Add(ConvertBasicChat(basicChat));
            }

            return response;
        }

        private Chat ConvertBasicChat(BasicChat basicChat)
        {
            Chat chat = new Chat();
            chat.Active = basicChat.Active;
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
        private void PushFileToAmazonAWS(PushFileRequest request, FileUploadResponse fileUploadResponse)
        {
            StringContent awsaccesskeyidContent = null;
            StringContent aclContent = null;
            StringContent keyContent = null;
            StringContent signatureContent = null;
            StringContent policyContent = null;
            StringContent contentTypeContent = null;
            StringContent cacheControlContent = null;
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
                        request.FileStream.CopyTo(memoryStream);
                        fileContent = new ByteArrayContent(memoryStream.ToArray());
                    }

                    fileContent.Headers.Add(PushBulletConstants.AmazonHeaders.ContentType, PushBulletConstants.MimeTypes.OctetStream);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = string.Format("\"{0}\"", "file"),
                        FileName = string.Format("\"{0}\"", request.FileName)
                    };

                    multiPartCont.Add(fileContent);

                    using (var httpClient = new HttpClient())
                    {
                        Task<HttpResponseMessage> httpRequest = httpClient.PostAsync(fileUploadResponse.UploadUrl, multiPartCont);
                        HttpResponseMessage httpResponse = httpRequest.Result;

                        Task<string> xmlContentResponse = httpResponse.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(xmlContentResponse.Result))
                        {
                            throw new Exception(xmlContentResponse.Result);
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
                if (awsaccesskeyidContent != null)
                {
                    awsaccesskeyidContent.Dispose();
                }
                if (aclContent != null)
                {
                    aclContent.Dispose();
                }
                if (keyContent != null)
                {
                    keyContent.Dispose();
                }
                if (signatureContent != null)
                {
                    signatureContent.Dispose();
                }
                if (policyContent != null)
                {
                    policyContent.Dispose();
                }
                if (contentTypeContent != null)
                {
                    contentTypeContent.Dispose();
                }
                if (cacheControlContent != null)
                {
                    cacheControlContent.Dispose();
                }
                if (fileContent != null)
                {
                    fileContent.Dispose();
                }
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
                Name = string.Format("\"{0}\"", name)
            };
            return content;
        }


        private void SetEncryptionKey(string encryptionPassword)
        {
            var currentUser = CurrentUsersInformation();
            if (currentUser == null)
            {
                throw new Exception("Could not retrieve the current user information to create an encryption key.");
            }

            _encryptionKey = Encryption.EncryptionUtility.GenerateKey(currentUser.Iden, encryptionPassword);
        }

        #endregion private methods
    }
}