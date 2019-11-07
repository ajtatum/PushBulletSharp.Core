using System.Runtime.Serialization;

namespace PushBulletSharp.Core.Models.Responses.WebSocket
{
    /// <summary>
    /// WebSocket Response
    /// </summary>
    [DataContract]
    public class WebSocketResponse
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the subtype.
        /// </summary>
        /// <value>
        /// The subtype.
        /// </value>
        [DataMember(Name = "subtype")]
        public string Subtype { get; set; }

        /// <summary>
        /// Gets or sets the push.
        /// </summary>
        /// <value>
        /// The push.
        /// </value>
        [DataMember(Name = "push")]
        public IEphemeral Push { get; set; }
    }
}