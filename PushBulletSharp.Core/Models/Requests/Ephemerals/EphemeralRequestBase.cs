using System.Runtime.Serialization;

namespace PushBulletSharp.Core.Models.Requests.Ephemerals
{
    /// <summary>
    /// Ephemeral Request Base
    /// </summary>
    [DataContract]
    public abstract class EphemeralRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EphemeralRequestBase"/> class.
        /// </summary>
        public EphemeralRequestBase()
        {
            Type = "push";
        }

        /// <summary>
        /// Type
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
