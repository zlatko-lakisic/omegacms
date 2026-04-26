using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.Helpers.Core.Templating
{
    /// <summary>
    /// Encapsulates information about an email
    /// </summary>
    public class EmailTemplate
    {
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public string Subject { get; set; }
        /// <summary>
        /// Gets or sets the plain text.
        /// </summary>
        /// <value>The plain text.</value>
        public string Plaintext { get; set; }
        /// <summary>
        /// Gets or sets the HTML.
        /// </summary>
        /// <value>The HTML.</value>
        public string Html { get; set; }
    }
}
