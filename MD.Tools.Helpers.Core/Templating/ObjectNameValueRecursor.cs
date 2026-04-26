using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using MD.Tools.Helpers.Core.TypeConversion;

namespace MD.Tools.Helpers.Core.Templating
{
    /// <summary>
    /// Generates a list of key value pairs from and object recursively
    /// </summary>
    public class ObjectNameValueRecursor
    {
        private SortedList<string, object> _nameValuePairs;
        private HashSet<object> _recursed;

        private StringBuilder _keys;
        private string _keyFormat = "{1,-25} {0}\n";

        /// <summary>
        /// Gets or sets a value indicating whether [populate keys list].
        /// </summary>
        /// <value><c>true</c> if [populate keys list]; otherwise, <c>false</c>.</value>
        public bool PopulateKeysList
        {
            get
            {
                return _keys != null;
            }
            set
            {
                if (value)
                {
                    _keys = new StringBuilder();
                }
                else
                {
                    _keys = null;
                }
            }
        }

        /// <summary>
        /// Gets the keys list.
        /// </summary>
        /// <value>The keys list.</value>
        public string KeysList
        {
            get
            {
                if (_keys == null) return string.Empty;
                return _keys.ToString();
            }
        }

        /// <summary>
        /// A sorted list of name-value pairs containing the paths to leaf properties and their values
        /// </summary>
        public SortedList<string, object> NameValuePairs
        {
            get
            {
                if (null == _nameValuePairs)
                {
                    _nameValuePairs = new SortedList<string, object>(new KeyLengthComparer());
                }

                return _nameValuePairs;
            }
        }

        /// <summary>
        /// Recurses an object and adds the paths and values of leaf properties to the <see cref="NameValuePairs"/> collection
        /// </summary>
        /// <param name="objectToRecurse">An object to be recursed for leaf property paths and values</param>
        /// <param name="truncationPaths">A list of partial paths that will be removed from the beginning of all recursion results or null to ignore</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object")]
        public void Recurse(
            object objectToRecurse,
            IEnumerable<string> truncationPaths)
        {
            if (objectToRecurse is null)
            {
                throw new ArgumentNullException(nameof(objectToRecurse));
            }

            _recursed = new HashSet<object>();
            List<string> paths = new List<string>();
            if (truncationPaths != null) paths.AddRange(truncationPaths);
            paths.Sort(new KeyLengthComparer());
            InnerRecurse(String.Empty, objectToRecurse, objectToRecurse.GetType(), paths);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void InnerRecurse(
            string parentPath,
            object objectToRecurse,
            Type type,
            IEnumerable<string> truncationPaths)
        {
            string path = String.Empty;
            object value = null;

            if (_recursed.Contains(objectToRecurse) && objectToRecurse != null) return;

            _recursed.Add(objectToRecurse);
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                try
                {
                    path = "{0}.{1}".ToFormattedString( parentPath, propertyInfo.Name).Trim('.');

                    if (propertyInfo.PropertyType.IsEnum ||
                        propertyInfo.PropertyType.IsValueType ||
                        propertyInfo.PropertyType == typeof(string))
                    {
                        if (null != truncationPaths)
                        {
                            foreach (string truncationPath in truncationPaths)
                            {
                                if (path.StartsWith(truncationPath, StringComparison.OrdinalIgnoreCase))
                                {
                                    path = path.Remove(0, truncationPath.Length).Trim('.');
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(path))
                        {
                            if (objectToRecurse != null) value = propertyInfo.GetValue(objectToRecurse, null);

                            NameValuePairs.Add(path, value);
                            if (_keys != null)
                            {
                                Type pt = propertyInfo.PropertyType;
                                if (pt.IsGenericType && pt.GetGenericTypeDefinition() == typeof(Nullable<>))
                                {
                                    pt = pt.GetGenericArguments()[0];
                                }
                                _keys.AppendFormat(CultureInfo.InvariantCulture, _keyFormat, path, pt.Name);
                            }
                        }

                    }
                    else if (
                        !
                        (
                            propertyInfo.PropertyType.FullName.StartsWith("System", StringComparison.OrdinalIgnoreCase)
                            ||
                            propertyInfo.PropertyType.FullName.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase)
                            ||
                            typeof(System.Collections.IEnumerable).IsAssignableFrom(propertyInfo.PropertyType)
                        )
                        &&
                        (
                            propertyInfo.PropertyType.IsClass ||
                            propertyInfo.PropertyType.IsInterface
                        )
                        )
                    {
                        if (objectToRecurse != null) value = propertyInfo.GetValue(objectToRecurse, null);
                        InnerRecurse(path, value, propertyInfo.PropertyType, truncationPaths);

                    }
                }
                catch (TemplatingException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new TemplatingException(ex, propertyInfo);
                }

            }
        }
    }
}
