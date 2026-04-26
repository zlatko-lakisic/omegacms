using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Collections.Specialized;
using System.Web;

namespace MD.Tools.Helpers.Core.TypeConversion
{
    public static partial class Utility
    {
        private static Uri _known = new Uri("http://tempuri.org/");

        /// <summary>
        /// Converts to absolute URI.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns>A absolute uri</returns>
        public static string ToAbsolutePath(this string link)
        {
            return ToAbsolutePath(link, "/");
        }

        /// <summary>
        /// Converts to absolute URI.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="applicationPath">The application path.</param>
        /// <returns>An absolute uri</returns>
        public static string ToAbsolutePath(this string link, string applicationPath)
        {
            return ToAbsolutePath(link, applicationPath, null);
        }

        /// <summary>
        /// Converts to absolute URI.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="applicationPath">The application path.</param>
        /// <param name="stripDomains">The strip domains.</param>
        /// <returns>An absolute uri</returns>
        public static string ToAbsolutePath(this string link, string applicationPath, IEnumerable<string> stripDomains)
        {
            if (string.IsNullOrEmpty(applicationPath))
            {
                throw new ArgumentOutOfRangeException(nameof(applicationPath));
            }

            if (string.IsNullOrEmpty(link)) return applicationPath;
            List<string> domainsToRemove = new List<string>();
            if (stripDomains != null) domainsToRemove.AddRange(stripDomains);
            if (!link.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                link=link.Replace("//","/", StringComparison.OrdinalIgnoreCase);
            Uri ub = new Uri(_known, link);
            if (string.Equals(ub.ToString(), link, StringComparison.OrdinalIgnoreCase)
                || string.Equals(ub.ToString().Trim('/'), link.Trim('/'), StringComparison.OrdinalIgnoreCase))
            {
                if (domainsToRemove.Count == 0 || domainsToRemove.Contains(ub.Host, StringComparer.OrdinalIgnoreCase)) return ub.PathAndQuery;
                return ub.ToString();
            }
            link = string.Concat("~/", link.TrimStart('/'));
            return string.Concat(applicationPath.TrimEnd(new char[] { '/' }), '/', link.TrimStart(new char[] { '/', '~' }));
        }

        /// <summary>
        /// Amends the query string for a uri
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The amended url</returns>
        /// <remarks>If the <paramref name="value"/> is <c>null</c> or empty the key value is removed</remarks>
        /// <exception cref="ArgumentNullException">Thrown if <see parmref="uri" /> or <paramref name="key"/> is <c>null</c>.</exception>
        public static Uri AmendQueryString(this Uri uri, string key, string value)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            UriBuilder ub = new UriBuilder(uri);
            NameValueCollection nvc = HttpUtility.ParseQueryString(uri.Query);
            if (string.IsNullOrEmpty(value))
            {
                nvc.Remove(key);
            }
            else
            {
                nvc[key] = value;
            }
            ub.Query = nvc.ToString();
            return ub.Uri;
        }

        /// <summary>
        /// Amends the query string using the provided updated parameters
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="alteredParameters">The altered parameters.</param>
        /// <returns>The amended url</returns>
        public static Uri AmendQueryString(this Uri uri, NameValueCollection alteredParameters)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));
            if (alteredParameters == null) throw new ArgumentNullException(nameof(alteredParameters));
            UriBuilder ub = new UriBuilder(uri);
            NameValueCollection nvc = HttpUtility.ParseQueryString(uri.Query);
            foreach (string key in alteredParameters.Keys)
            {
                string value = alteredParameters[key];
                if (string.IsNullOrEmpty(value))
                {
                    nvc.Remove(key);
                }
                else
                {
                    nvc[key] = value;
                }
            }
            ub.Query = nvc.ToString();
            return ub.Uri;
        }

        /// <summary>
        /// Gets the queue from URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string ToQueue(this Uri uri)
        {
            if(uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            string queue = string.Empty;
            if (!((uri.Segments.Length == 3) || (uri.Segments.Length == 2))) throw new NotSupportedException("Cannot convert '{0}' to value MSMQ name".ToFormattedString(uri));
            if (uri.Segments[1] == "private/")
            {
                return (@".\private$\" + uri.Segments[2]);
            }
            queue = uri.Host;
            foreach (string segment in uri.Segments)
            {
                if (segment != "/")
                {
                    string localSegment = segment;
                    if (segment[segment.Length - 1] == '/')
                    {
                        localSegment = segment.Remove(segment.Length - 1);
                    }
                    queue = queue + @"\" + localSegment;
                }
            }
            return queue;
        }
    }

}
