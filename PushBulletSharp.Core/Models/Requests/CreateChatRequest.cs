using System.Runtime.Serialization;

namespace PushBulletSharp.Core.Models.Requests
{
    /// <summary>
    /// Create Chat Request
    /// </summary>
    [DataContract]
    public class CreateChatRequest
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}