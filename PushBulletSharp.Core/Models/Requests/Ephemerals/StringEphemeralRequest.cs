using System.Runtime.Serialization;

namespace PushBulletSharp.Core.Models.Requests.Ephemerals
{
    /// <summary>
    /// String Ephemeral Request
    /// </summary>
    [DataContract]
    public class StringEphemeralRequest : EphemeralRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringEphemeralRequest"/> class.
        /// </summary>
        public StringEphemeralRequest()
        {
            Type = "push";
        }

        /// <summary>
        /// Gets or sets the push.
        /// </summary>
        /// <value>
        /// The push.
        /// </value>
        [DataMember(Name = "push")]
        public string Push { get; set; }
    }
}