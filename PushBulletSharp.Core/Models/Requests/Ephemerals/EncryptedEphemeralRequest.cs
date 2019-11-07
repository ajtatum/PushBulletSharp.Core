﻿using System.Runtime.Serialization;

namespace PushBulletSharp.Core.Models.Requests.Ephemerals
{
    /// <summary>
    /// Encrypted Ephemeral Request
    /// </summary>
    [DataContract]
    public class EncryptedEphemeralRequest : EphemeralRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedEphemeralRequest"/> class.
        /// </summary>
        public EncryptedEphemeralRequest() : base()
        {
        }

        /// <summary>
        /// Gets or sets the push.
        /// </summary>
        /// <value>
        /// The push.
        /// </value>
        [DataMember(Name = "push")]
        public EncryptedEphemeralMessage Push { get; set; }
    }
}
