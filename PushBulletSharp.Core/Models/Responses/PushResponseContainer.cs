﻿using System.Collections.Generic;

namespace PushBulletSharp.Core.Models.Responses
{
    /// <summary>
    /// Push Response Container
    /// </summary>
    public class PushResponseContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PushResponseContainer"/> class.
        /// </summary>
        public PushResponseContainer()
        {
            Pushes = new List<PushResponse>();
        }

        /// <summary>
        /// Gets or sets the pushes.
        /// </summary>
        /// <value>
        /// The pushes.
        /// </value>
        public List<PushResponse> Pushes { get; set; }

        /// <summary>
        /// Gets or sets the cursor.
        /// </summary>
        /// <value>
        /// The cursor.
        /// </value>
        public string Cursor { get; set; }
    }
}