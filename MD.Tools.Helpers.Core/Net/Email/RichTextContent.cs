using System.Net.Mime;
using System.Text;

namespace MD.Tools.Helpers.Core.Net.Email
{
    /// <summary>
    /// Implements email content in rich text format.
    /// The PlainTextContent class is used to allow rich text content to be sent from an IEmailSender
    /// </summary>
    public class RichTextContent : EmailContent
    {
        /// <summary>
        /// Initialises an instance of RichTextContent using body content and an encoding
        /// </summary>
        /// <param name="body">A string containing body content</param>
        /// <param name="encoding">A System.Text.Encoding indicating the encoding utilised in the body</param>
        public RichTextContent(string body, Encoding encoding)
            : base(body, encoding)
        {
        }

        /// <summary>
        /// The MIME content type of the body (text/rtf)
        /// </summary>
        public override ContentType ContentType
        {
            get
            {
                return new ContentType(MediaTypeNames.Text.RichText);
            }
        }
    }

}
