using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;
using MD.Tools.Helpers.Core.TypeConversion;

namespace MD.Tools.Helpers.Core.Templating
{
    /// <summary>
    /// Wraps templating exceptions
    /// </summary>
    [Serializable]
    public class TemplatingException : Exception
    {
        private const string ErrorMessage = "There was an error processing {0} on {1}";

        private const string TemplateErrorMessage = "There was an error populating tempate {0} allowed values are: \n{1}";

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplatingException"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="propertyInfo">The property info.</param>
#pragma warning disable CA1062 // Validate arguments of public methods
        public TemplatingException(Exception exception, PropertyInfo propertyInfo) : base(ErrorMessage.ToFormattedString(propertyInfo.Name, propertyInfo.DeclaringType.Name), exception)
#pragma warning restore CA1062 // Validate arguments of public methods
        {
            if (propertyInfo is null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplatingException"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="template">The template.</param>
        /// <param name="allowedValues">The allowed values.</param>
        public TemplatingException(FormatException exception, string template, string[] allowedValues) : base(TemplateErrorMessage.ToFormattedString( template, string.Join(Environment.NewLine, allowedValues)),exception) { }

    }
}
