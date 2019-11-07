namespace PushBulletSharp.Core.Constants
{
    /// <summary>
    /// PushBullet Constants
    /// </summary>
    public class PushBulletConstants
    {
        /// <summary>
        /// https://api.pushbullet.com/v2/
        /// </summary>
        public const string BaseUrl = "https://api.pushbullet.com/v2/";
        /// <summary>
        /// https://api.pushbullet.com/
        /// </summary>
        public const string BaseUrlNonVersion = "https://api.pushbullet.com/";

        /// <summary>
        /// Status Code Exceptions
        /// </summary>
        public class StatusCodeExceptions
        {
            /// <summary>
            /// 400 Bad Request - Usually this results from missing a required parameter.
            /// </summary>
            public const string BadRequest = "400 Bad Request - Usually this results from missing a required parameter.";
            /// <summary>
            /// 401 Unauthorized - No valid access token provided.
            /// </summary>
            public const string Unauthorized = "401 Unauthorized - No valid access token provided.";
            /// <summary>
            /// 403 Forbidden - The access token is not valid for that request.
            /// </summary>
            public const string Forbidden = "403 Forbidden - The access token is not valid for that request.";
            /// <summary>
            /// 404 Not Found - The requested item doesn't exist.
            /// </summary>
            public const string NotFound = "404 Not Found - The requested item doesn't exist.";
            /// <summary>
            /// 429 Too Many Requests - You have been ratelimited for making too many requests to the server.
            /// </summary>
            public const string TooManyRequests = "429 Too Many Requests - You have been ratelimited for making too many requests to the server.";
            /// <summary>
            /// Something went wrong on PushBullet's side.
            /// </summary>
            public const string FiveHundredXX = "{0} {1} - Something went wrong on PushBullet's side.";
            /// <summary>
            /// Error getting data from PushBullet.
            /// </summary>
            public const string Default = "{0} {1} - Error getting data from PushBullet.";
        }

        /// <summary>
        /// Http Messages
        /// </summary>
        public class HttpMethods
        {
            /// <summary>
            /// GET
            /// </summary>
            public const string GET = "GET";
            /// <summary>
            /// POST
            /// </summary>
            public const string POST = "POST";
            /// <summary>
            /// DELETE
            /// </summary>
            public const string DELETE = "DELETE";
        }

        /// <summary>
        /// Push Request Error Messages
        /// </summary>
        public class PushRequestErrorMessages
        {
            /// <summary>
            /// The device iden property for the request is empty.
            /// </summary>
            public const string EmptyDeviceIdenProperty = "The device iden property for the request is empty.";
            /// <summary>
            /// The type property for the request is empty. Not even sure how that happened.
            /// </summary>
            public const string EmptyTypeProperty = "The type property for the request is empty. Not even sure how that happened.";
            /// <summary>
            /// The email property for the request is empty. This is only a problem because both the device iden and client iden were empty, too.
            /// </summary>
            public const string EmptyEmailProperty = "The email property for the request is empty. This is only a problem because both the device iden and client iden were empty, too.";
        }


        /// <summary>
        /// PushNote Request Error Messages
        /// </summary>
        public class PushNoteRequestErrorMessages
        {
            /// <summary>
            /// The title property for the note request is empty. Please provide a title.
            /// </summary>
            public const string EmptyTitleProperty = "The title property for the note request is empty. Please provide a title.";
            /// <summary>
            /// The body property for the note request is empty. Please provide a body.
            /// </summary>
            public const string EmptyBodyProperty = "The body property for the note request is empty. Please provide a body.";
        }


        /// <summary>
        /// PushLink Error Messages
        /// </summary>
        public class PushLinkErrorMessages
        {
            /// <summary>
            /// The title property for the link request is empty. Please provide a title.
            /// </summary>
            public const string EmptyTitleProperty = "The title property for the link request is empty. Please provide a title.";
            /// <summary>
            /// The url property for the link request is empty. Please provide a url.
            /// </summary>
            public const string EmptyUrlProperty = "The url property for the link request is empty. Please provide a url.";
        }


        /// <summary>
        /// PushFile Error Messages
        /// </summary>
        public class PushFileErrorMessages
        {
            /// <summary>
            /// There was an error while making a request to upload the file to pushbullet.
            /// </summary>
            public const string ErrorMakingFileUploadRequest = "There was an error while making a request to upload the file to pushbullet.";
            /// <summary>
            /// The file_name property for the file request is empty. Please provide a file_name.
            /// </summary>
            public const string EmptyFileNameProperty = "The file_name property for the file request is empty. Please provide a file_name.";
            /// <summary>
            /// The file_type property for the file request is empty. Please provide a file_type.
            /// </summary>
            public const string EmptyFileTypeProperty = "The file_type property for the file request is empty. Please provide a file_type.";
            /// <summary>
            /// The file_stream property for the file request is empty. Please provide a valid file_stream.
            /// </summary>
            public const string EmptyFileStreamProperty = "The file_stream property for the file request is empty. Please provide a valid file_stream.";
            /// <summary>
            /// The body property for the file request is empty. While this is optional, the way you are calling the PushFile method requires you to provide a body.
            /// </summary>
            public const string EmptyBodyProperty = "The body property for the file request is empty. While this is optional, the way you are calling the PushFile method requires you to provide a body.";
            /// <summary>
            /// The file_stream for the file request can't be read. Please provide a valid file_stream.
            /// </summary>
            public const string CantReadFileStreamProperty = "The file_stream for the file request can't be read. Please provide a valid file_stream.";
        }


        /// <summary>
        /// PushResponse Filter Error Messages
        /// </summary>
        public class PushResponseFilterErrorMessages
        {
            /// <summary>
            /// You must provide a modified date in your filter.
            /// </summary>
            public const string MissingDateModifiedError = "You must provide a modified date in your filter.";
        }


        /// <summary>
        /// CreateContact Error Messages
        /// </summary>
        public class CreateContactErrorMessages
        {
            /// <summary>
            /// The name property for creating a new contact is empty. Please provide a name.
            /// </summary>
            public const string ErrorNameProperty = "The name property for creating a new contact is empty. Please provide a name.";
            /// <summary>
            /// The email property for creating a new contact is empty. Please provide an email.
            /// </summary>
            public const string ErrorEmailProperty = "The email property for creating a new contact is empty. Please provide an email.";
        }


        /// <summary>
        /// UpdateContact Error Messages
        /// </summary>
        public class UpdateContactErrorMessages
        {
            /// <summary>
            /// The contact_iden property for updating a contact is empty. Please provide a valid contact_iden.
            /// </summary>
            public const string ErrorContactIdenProperty = "The contact_iden property for updating a contact is empty. Please provide a valid contact_iden.";
            /// <summary>
            /// The name property for updating a contact is empty. Please provide a new name.
            /// </summary>
            public const string ErrorNameProperty = "The name property for updating a contact is empty. Please provide a new name.";
        }


        /// <summary>
        /// DeleteContact Error Messages
        /// </summary>
        public class DeleteContactErrorMessages
        {
            /// <summary>
            /// The contact_iden property for deleting a contact is empty. Please provide a valid contact_iden.
            /// </summary>
            public const string ErrorContactIdenProperty = "The contact_iden property for deleting a contact is empty. Please provide a valid contact_iden.";
        }


        /// <summary>
        /// CreateChat Error Messages
        /// </summary>
        public class CreateChatErrorMessages
        {
            /// <summary>
            /// The email property for creating a new chat is empty. Please provide an email.
            /// </summary>
            public const string ErrorEmailProperty = "The email property for creating a new chat is empty. Please provide an email.";
        }


        /// <summary>
        /// UpdateChat Error Messages
        /// </summary>
        public class UpdateChatErrorMessages
        {
            /// <summary>
            /// The iden property for updating the chat is empty. Please provide a valid iden.
            /// </summary>
            public const string ErrorIdenProperty = "The iden property for updating the chat is empty. Please provide a valid iden.";
        }


        /// <summary>
        /// DeleteChat Error Messages
        /// </summary>
        public class DeleteChatErrorMessages
        {
            /// <summary>
            /// The chat iden property for deleting the chat is empty. Please provide a valid chat iden.
            /// </summary>
            public const string ErrorIdenProperty = "The chat iden property for deleting the chat is empty. Please provide a valid chat iden.";
        }


        /// <summary>
        /// OAuth Error Messages
        /// </summary>
        public class OAuthErrorMessages
        {
            /// <summary>
            /// Web Exception
            /// </summary>
            public const string WebExceptionFormat = "Status code: {0} while trying to request an OAuth token. {1}";
        }


        /// <summary>
        /// Headers Constants
        /// </summary>
        public class HeadersConstants
        {
            /// <summary>
            /// Authorization Key
            /// </summary>
            public const string AuthorizationKey = "Authorization";
            /// <summary>
            /// Authorization Value
            /// </summary>
            public const string AuthorizationValue = "Bearer {0}";
        }


        /// <summary>
        /// Type Constants
        /// </summary>
        public class TypeConstants
        {
            /// <summary>
            /// Note
            /// </summary>
            public const string Note = "note";
            /// <summary>
            /// Link
            /// </summary>
            public const string Link = "link";
            /// <summary>
            /// File
            /// </summary>
            public const string File = "file";
        }


        /// <summary>
        /// Users Urls
        /// </summary>
        public class UsersUrls
        {
            /// <summary>
            /// users/me
            /// </summary>
            public const string Me = "users/me";
        }


        /// <summary>
        /// Devices Urls
        /// </summary>
        public class DevicesUrls
        {
            /// <summary>
            /// devices
            /// </summary>
            public const string Me = "devices";
        }


        /// <summary>
        /// Pushes Urls
        /// </summary>
        public class PushesUrls
        {
            /// <summary>
            /// Pushes
            /// </summary>
            public const string Pushes = "pushes";
        }


        /// <summary>
        /// Ephemerals Urls
        /// </summary>
        public class EphemeralsUrls
        {
            /// <summary>
            /// Ephemerals
            /// </summary>
            public const string Ephemerals = "ephemerals";
        }


        /// <summary>
        /// Chats Urls
        /// </summary>
        public class ChatsUrls
        {
            /// <summary>
            /// Chats
            /// </summary>
            public const string Chats = "chats";
        }


        /// <summary>
        /// File Urls
        /// </summary>
        public class FileUrls
        {
            /// <summary>
            /// Upload Request
            /// </summary>
            public const string UploadRequest = "upload-request";
            /// <summary>
            /// Amazon AWS
            /// </summary>
            public const string AmazonAWS = "https://s3.amazonaws.com/pushbullet-uploads";
        }


        /// <summary>
        /// Contacts Urls
        /// </summary>
        public class ContactsUrls
        {
            /// <summary>
            /// Contacts
            /// </summary>
            public const string Contacts = "contacts";
        }


        /// <summary>
        /// Subscription Urls
        /// </summary>
        public class SubscriptionUrls
        {
            /// <summary>
            /// subscriptions
            /// </summary>
            public const string Subscriptions = "subscriptions";
            /// <summary>
            /// channel-info
            /// </summary>
            public const string ChannelInfo = "channel-info";
        }


        /// <summary>
        /// OAuth Urls
        /// </summary>
        public class OAuthUrls
        {
            /// <summary>
            /// OAuth Token
            /// </summary>
            public const string OAuthToken = "oauth2/token";
        }


        /// <summary>
        /// Amazon Headers
        /// </summary>
        public class AmazonHeaders
        {
            /// <summary>
            /// Content Type
            /// </summary>
            public const string ContentType = "Content-Type";
            /// <summary>
            /// Cache Control
            /// </summary>
            public const string CacheControl = "Cache-Control";
            /// <summary>
            /// Cache Control Default Value
            /// </summary>
            public const string CacheControlDefaultValue = "max-age=31556926";
        }


        /// <summary>
        /// Mime Types
        /// </summary>
        public class MimeTypes
        {
            /// <summary>
            /// Json
            /// </summary>
            public const string Json = "application/json";
            /// <summary>
            /// Octet Stream
            /// </summary>
            public const string OctetStream = "application/octet-stream";
            /// <summary>
            /// Form Url Encoded
            /// </summary>
            public const string FormUrlEncoded = "application/x-www-form-urlencoded";
        }

        /// <summary>
        /// Defaults
        /// </summary>
        public class Defaults
        {
            /// <summary>
            /// OAuth
            /// </summary>
            public class OAuth
            {
                /// <summary>
                /// Default Grant Type
                /// </summary>
                public const string DefaultGrantType = "authorization_code";
            }
        }
    }
}