using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using MD.Tools.Helpers.Core.TypeConversion;

namespace MD.Tools.Helpers.Core.Templating
{
    /// <summary>
    /// Handles the population of templates using string.Format style and options
    /// </summary>
    public class TemplateProcessor
    {
        private static Regex _curlyBrace = new Regex(@"(\{)(?([^\d\{\}]+|(?=\})|(\d+\s))[^\}\{]*)(\})", System.Text.RegularExpressions.RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        private object _dto;
        private IEnumerable<string> _namesToIgnore;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateProcessor"/> class.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        public TemplateProcessor(object dataObject) : this(dataObject, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateProcessor"/> class.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="namesToIgnore">The names to ignore.</param>
        public TemplateProcessor(object dataObject, IEnumerable<string> namesToIgnore)
        {
            _dto = dataObject;
            _namesToIgnore = namesToIgnore;
        }

        private SortedList<string, object> _keysAndValues = new SortedList<string, object>();

        /// <summary>
        /// Gets the keys and values.
        /// </summary>
        /// <value>The keys and values.</value>
        public SortedList<string, object> KeysAndValues
        {
            get
            {
                if (_keysAndValues.Count == 0) _keysAndValues = GetTemplateKeysAndValues(_dto, _namesToIgnore);
                return _keysAndValues;
            }
        }

        /// <summary>
        /// Gets the template keys and values.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="namesToIgnore">The names to ignore.</param>
        /// <returns></returns>
        private static SortedList<string, object> GetTemplateKeysAndValues(object dataObject, IEnumerable<string> namesToIgnore)
        {
            ObjectNameValueRecursor recursor = new ObjectNameValueRecursor();
            recursor.PopulateKeysList = false;
            recursor.Recurse(dataObject, namesToIgnore);
            SortedList<string, object> values = recursor.NameValuePairs;
            return values;
        }

        private static string ReplaceString(string value, string stringToReplace, string replacementString)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (string.IsNullOrEmpty(stringToReplace)) return value;
            int index = -1;
            do
            {
                index = value.IndexOf(stringToReplace, index + 1, StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                {
                    value = value.Remove(index, stringToReplace.Length);
                    value = value.Insert(index, replacementString);

                }
            }
            while (index != -1);
            return value;
        }

        /// <summary>
        /// Populates the template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="formatter">The formatter.</param>
        /// <returns></returns>
        public string PopulateTemplate(string template, IFormatProvider formatter)
        {
            StringBuilder sb = new StringBuilder();
            PopulateTemplate(template, formatter, sb);
            return sb.ToString();
        }

        /// <summary>
        /// Populates the template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="output">The output.</param>
        public void PopulateTemplate(string template, IFormatProvider formatter, StringBuilder output)
        {
            PopulateTemplate(template, formatter, output, KeysAndValues);
        }

        /// <summary>
        /// Populates the template.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="output">The output.</param>
        /// <param name="values">The values.</param>
        public static void PopulateTemplate(string template, IFormatProvider formatter, StringBuilder output, SortedList<string, object> values)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (string.IsNullOrEmpty(template)) return;
            String newMask = template;
            List<object> vals = new List<object>();
            for (int i = 0; i < values.Count; i++)
            {
                string key = values.Keys[i];

                newMask = ReplaceString(newMask,
                    "{{{0}".ToFormattedString( key)
                    , "{{{0}".ToFormattedString( i)
                    );
                vals.Add(values[key]);
            }
            string mk = _curlyBrace.Replace(newMask.ToString(CultureInfo.InvariantCulture), "{$&}");
            try
            {
                output.AppendFormat(formatter, mk, vals.ToArray());
            }
            catch (FormatException fex)
            {
                throw new TemplatingException(fex, mk, values.Keys.ToArray<string>());
            }
        }

        /// <summary>
        /// Creates the mail message.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="formatter">The formatter.</param>
        /// <returns>A Multi part email based on the provided template</returns>
        public System.Net.Mail.MailMessage CreateMailMessage(EmailTemplate template, IFormatProvider formatter)
        {
            if (template is null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.Subject = PopulateTemplate(template.Subject, formatter);
            mailMessage.BodyEncoding = Encoding.UTF8;
           
            StringBuilder plainText = new StringBuilder();
            PopulateTemplate(template.Plaintext, formatter, plainText);
            mailMessage.AlternateViews.Add(System.Net.Mail.AlternateView.CreateAlternateViewFromString(plainText.ToString()));
           
            StringBuilder html = new StringBuilder();
            PopulateTemplate(template.Html, formatter, html);
            mailMessage.AlternateViews.Add(System.Net.Mail.AlternateView.CreateAlternateViewFromString(html.ToString(), new System.Net.Mime.ContentType("text/html")));
            
            return mailMessage;

        }

    }
}
