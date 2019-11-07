﻿using System;
using System.Runtime.Serialization;

namespace PushBulletSharp.Core.Models.Responses
{
    [DataContract]
    public class Chat
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Chat"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "active")]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        [DataMember(Name = "created")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the iden.
        /// </summary>
        /// <value>
        /// The iden.
        /// </value>
        [DataMember(Name = "iden")]
        public string Iden { get; set; }

        /// <summary>
        /// Gets or sets the modified.
        /// </summary>
        /// <value>
        /// The modified.
        /// </value>
        [DataMember(Name = "modified")]
        public DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets the with.
        /// </summary>
        /// <value>
        /// The with.
        /// </value>
        [DataMember(Name = "with")]
        public ChatContact With { get; set; }
    }
}