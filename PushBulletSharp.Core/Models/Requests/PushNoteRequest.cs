using System.Runtime.Serialization;
using PushBulletSharp.Core.Constants;

namespace PushBulletSharp.Core.Models.Requests
{
    [DataContract]
    public class PushNoteRequest : PushRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PushNoteRequest"/> class.
        /// </summary>
        public PushNoteRequest()
        {
            Type = PushBulletConstants.TypeConstants.Note;
        }

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
    }
}