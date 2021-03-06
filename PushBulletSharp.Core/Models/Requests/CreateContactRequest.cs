﻿using System.Runtime.Serialization;

namespace PushBulletSharp.Core.Models.Requests
{
    /// <summary>
    /// Create Contact Request
    /// </summary>
    [DataContract]
    public class CreateContactRequest
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Name = "name")]
        public string Name { get; set; }

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