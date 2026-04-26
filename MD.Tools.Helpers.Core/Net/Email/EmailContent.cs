using System;
using System.Net.Mime;
using System.Text;

namespace MD.Tools.Helpers.Core.Net.Email
{
    /// <summary>
    /// Base class for email content.
    /// The EmailContent class is used to allow multimedia content to be sent from an <see ref="IEmailSender" />
    /// </summary>
    public abstract class EmailContent
    {
        /// <summary>
        /// Initialises an instance of <see ref="EmailContent" /> using body content and an encoding
        /// </summary>
        /// <param name="body">A string containing body content</param>
        /// <param name="encoding">A <see ref="System.Text.Encoding" /> indicating the encoding utilised in the body</param>
        /// <exception cref="System.ArgumentNullException">Throw when <paramref name="body"/> is null</exception>
        protected EmailContent(string body, Encoding encoding)
        {
            if (String.IsNullOrEmpty(body))
            {
                throw new ArgumentNullException(nameof(body));
            }

            Body = body;
            Encoding = encoding;
        }

        /// <summary>
        /// The body content
        /// </summary>
        public string Body { get; protected set; }

        /// <summary>
        /// The encoding utilised in the body
        /// </summary>
        public Encoding Encoding { get; protected set; }

        /// <summary>
        /// The MIME content type of the body
        /// </summary>
        public abstract ContentType ContentType { get; }
    }

}
