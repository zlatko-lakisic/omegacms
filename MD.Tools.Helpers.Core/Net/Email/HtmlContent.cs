using System.Net.Mime;
using System.Text;

namespace MD.Tools.Helpers.Core.Net.Email
{
    /// <summary>
    /// Implements email content in HTML format.
    /// The <see ref="HtmlContent" /> class is used to allow HTML content to be sent from an <see ref="IEmailSender" />
    /// </summary>
    public class HtmlContent : EmailContent
    {
        /// <summary>
        /// Initialises an instance of <see ref="HtmlContent" /> using body content and an encoding
        /// </summary>
        /// <param name="body">A string containing body content</param>
        /// <param name="encoding">A System.Text.Encoding indicating the encoding utilised in the body</param>
        public HtmlContent(string body, Encoding encoding)
            : base(body, encoding)
        {
        }

        /// <summary>
        /// The MIME content type of the body (text/html)
        /// </summary>
        public override ContentType ContentType
        {
            get
            {
                return new ContentType(MediaTypeNames.Text.Html);
            }
        }
    }

}
