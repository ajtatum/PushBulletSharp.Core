﻿using System.Runtime.Serialization;

namespace PushBulletSharp.Core.Models.Responses.Ephemerals
{
    /// <summary>
    /// Notification Ephemeral
    /// </summary>
    [DataContract]
    public class NotificationEphemeral : IEphemeral
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
        /// Gets or sets the name of the package.
        /// </summary>
        /// <value>
        /// The name of the package.
        /// </value>
        [DataMember(Name = "package_name")]
        public string PackageName { get; set; }
        
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

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>
        /// The icon. Base 64 encoded icon
        /// </value>
        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        [DataMember(Name = "body")]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        [DataMember(Name = "application_name")]
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NotificationEphemeral"/> is dismissable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if dismissable; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "dismissable")]
        public bool Dismissable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has root.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has root; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "has_root")]
        public bool HasRoot { get; set; }

        /// <summary>
        /// Client Version
        /// </summary>
        [DataMember(Name = "client_version")]
        public int ClientVersion { get; set; }
    }
}