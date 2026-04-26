using System.Net.Mime;
using System.Text;

namespace MD.Tools.Helpers.Core.Net.Email
{
    /// <summary>
    /// Implements <see ref="EmailContent"/> in plain text format.
    /// The <see ref="PlaintextContent" /> class is used to allow plain text content to be sent from an <see ref="IEmailSender" />
    /// </summary>
    public class PlaintextContent : EmailContent
    {
        /// <summary>
        /// Initialises an instance of <see ref="PlainTextContent" /> using body content and an encoding
        /// </summary>
        /// <param name="body">A string containing body content</param>
        /// <param name="encoding">A <see ref="System.Text.Encoding" /> indicating the encoding utilised in the body</param>
        public PlaintextContent(string body, Encoding encoding)
            : base(body, encoding)
        {
        }

        /// <summary>
        /// The MIME content type of the body (text/plain)
        /// </summary>
        public override ContentType ContentType
        {
            get
            {
                return new ContentType(MediaTypeNames.Text.Plain);
            }
        }
    }

}
