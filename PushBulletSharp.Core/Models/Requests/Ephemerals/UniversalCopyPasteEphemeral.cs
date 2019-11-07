﻿using System.Runtime.Serialization;

namespace PushBulletSharp.Core.Models.Requests.Ephemerals
{
    /// <summary>
    /// Universal Copy Paste Ephemeral
    /// </summary>
    [DataContract]
    public class UniversalCopyPasteEphemeral : IEphemeral
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalCopyPasteEphemeral"/> class.
        /// </summary>
        public UniversalCopyPasteEphemeral()
        {
            Type = "clip";
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the source user iden.
        /// </summary>
        /// <value>
        /// The source user iden.
        /// </value>
        [DataMember(Name = "source_user_iden")]
        public string SourceUserIden { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        [DataMember(Name = "body")]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the source device iden.
        /// </summary>
        /// <value>
        /// The source device iden.
        /// </value>
        [DataMember(Name = "source_device_iden")]
        public string SourceDeviceIden { get; set; }
    }
}