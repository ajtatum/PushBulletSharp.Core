﻿using System.Runtime.Serialization;

namespace PushBulletSharp.Core.Models.Requests.Ephemerals
{
    [DataContract]
    public class DismissalEphemeral : IEphemeral
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DismissalEphemeral"/> class.
        /// </summary>
        public DismissalEphemeral()
        {
            Type = "dismissal";
            PackageName = "com.pushbullet.android";
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
        /// Gets or sets the name of the package.
        /// </summary>
        /// <value>
        /// The name of the package.
        /// </value>
        [DataMember(Name = "package_name")]
        public string PackageName { get; set; }

        /// <summary>
        /// Gets or sets the source user iden.
        /// </summary>
        /// <value>
        /// The source user iden.
        /// </value>
        [DataMember(Name = "source_user_iden")]
        public string SourceUserIden { get; set; }

        /// <summary>
        /// Gets or sets the notification identifier.
        /// </summary>
        /// <value>
        /// The notification identifier.
        /// </value>
        [DataMember(Name = "notification_id")]
        public string NotificationId { get; set; }

        /// <summary>
        /// Gets or sets the notification tag.
        /// </summary>
        /// <value>
        /// The notification tag.
        /// </value>
        [DataMember(Name = "notification_tag")]
        public string NotificationTag { get; set; }
    }
}