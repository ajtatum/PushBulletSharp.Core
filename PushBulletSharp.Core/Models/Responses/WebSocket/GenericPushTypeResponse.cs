using System.Runtime.Serialization;

namespace PushBulletSharp.Core.Models.Responses.WebSocket
{
    /// <summary>
    /// Generic Push Type Response
    /// </summary>
    [DataContract]
    public class GenericPushTypeResponse
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}