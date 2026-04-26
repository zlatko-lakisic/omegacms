using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;

namespace MD.Tools.Helpers.Core.TypeConversion
{
    public static partial class Utility
    {
        /// <summary>
        /// Toes the basic XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToBasicXml<T>(this T value) where T : class
        {
            return value.ToBasicXml(System.Xml.Formatting.Indented);
        }

        /// <summary>
        /// Gets the basic properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="formatting">The formatting.</param>
        /// <returns></returns>
        public static string ToBasicXml<T>(this T value, System.Xml.Formatting formatting) where T : class
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture))
            {
                using (System.Xml.XmlTextWriter xw = new System.Xml.XmlTextWriter(sw))
                {
                    xw.Formatting = formatting;
                    xw.WriteStartElement(value.GetType().Name);
                    xw.WriteAttributeString("type", value.GetType().FullName);
                    xw.WriteStartElement("Properties");
                    foreach (PropertyInfo pi in value.GetType().GetProperties())
                    {
                        if (pi.PropertyType.IsValueType || pi.PropertyType.IsPrimitive || typeof(string).IsAssignableFrom(pi.PropertyType))
                        {
                            xw.WriteStartElement(pi.Name);
                            xw.WriteAttributeString("type", pi.PropertyType.FullName);
                            xw.WriteString(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", pi.GetValue(value, null)));
                            xw.WriteEndElement();
                        }
                    }
                    xw.WriteEndElement(); //Properties
                    xw.WriteStartElement("Fields");
                    foreach (FieldInfo fi in value.GetType().GetFields())
                    {
                        if (fi.FieldType.IsValueType || fi.FieldType.IsPrimitive || typeof(string).IsAssignableFrom(fi.FieldType))
                        {
                            xw.WriteStartElement(fi.Name);
                            xw.WriteAttributeString("type", fi.FieldType.FullName);
                            xw.WriteString(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", fi.GetValue(value)));
                            xw.WriteEndElement();
                        }
                    }
                    xw.WriteEndElement(); //Fields
                    xw.WriteEndElement();
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// A basic factory method to recreate a DTO from basic XML information
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static T FromBasicXml<T>(this string xml) where T : class, new()
        {
            if (string.IsNullOrEmpty(xml)) return new T();
            T created = new T();
            System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Parse(xml);
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                if ((pi.PropertyType.IsValueType || pi.PropertyType.IsPrimitive || typeof(string).IsAssignableFrom(pi.PropertyType)) && pi.CanWrite)
                {
                    string xpath = @"/{0}/Properties/{1}".ToFormattedString(typeof(T).Name, pi.Name);
                    XElement propertyNode = doc.XPathSelectElement(xpath);
                    if (propertyNode != null)
                    {
                        if (pi.PropertyType == typeof(Guid))
                        {
                            pi.SetValue(created, new Guid(propertyNode.Value), null);
                        }
                        else
                        {
                            pi.SetValue(created, Convert.ChangeType(propertyNode.Value, pi.PropertyType, CultureInfo.InvariantCulture), null);
                        }
                    }
                }
            }
            foreach (FieldInfo fi in typeof(T).GetFields())
            {
                if (fi.FieldType.IsValueType || fi.FieldType.IsPrimitive || typeof(string).IsAssignableFrom(fi.FieldType))
                {
                    string xpath = @"/{0}/Fields/{1}".ToFormattedString(typeof(T).Name, fi.Name);
                    XElement fieldNode = doc.XPathSelectElement(xpath);
                    if (fieldNode != null)
                    {
                        if (fi.FieldType == typeof(Guid))
                        {
                            fi.SetValue(created, new Guid(fieldNode.Value));
                        }
                        else
                        {
                            fi.SetValue(created, Convert.ChangeType(fieldNode.Value, fi.FieldType, CultureInfo.InvariantCulture));
                        }
                    }
                }
            }
            return created;
        }

        /// <summary>
        /// Converts the class to an enumerable of one element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this T value)
        {
            if (value == null) yield break;
            yield return value;
        }

        /// <summary>
        /// Convers the object to a single element array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T[] AsArray<T>(this T value)
        {
            if (value == null) return Array.Empty<T>();
            return new T[] { value };
        }

        /// <summary>
        /// Creates a new instance of the target type and assigns any matching properties (and public fields) from the source object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static T CreateUsingProperties<T>(this object source) where T : class, new()
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            T newT = new T();
            System.Reflection.PropertyInfo[] newTpis = typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
            System.Reflection.FieldInfo[] newTfis = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField);
            foreach (System.Reflection.PropertyInfo pi in source.GetType()
                .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                .Where(p => newTpis.Where(np => string.Equals(np.Name, p.Name, StringComparison.OrdinalIgnoreCase)).Any()))
            {
                System.Reflection.PropertyInfo npi = newTpis.Where(np => string.Equals(np.Name, pi.Name, StringComparison.OrdinalIgnoreCase)).First();
                npi.SetValue(newT, pi.GetValue(source, null), null);
            }
            foreach (System.Reflection.PropertyInfo pi in source.GetType()
                .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                .Where(p => newTfis.Where(np => string.Equals(np.Name, p.Name, StringComparison.OrdinalIgnoreCase)).Any()))
            {
                System.Reflection.FieldInfo npi = newTfis.Where(np => string.Equals(np.Name, pi.Name, StringComparison.OrdinalIgnoreCase)).First();
                npi.SetValue(newT, pi.GetValue(source, null));
            }

            foreach (System.Reflection.FieldInfo fi in source.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField)
                .Where(p => newTpis.Where(np => string.Equals(np.Name, p.Name, StringComparison.OrdinalIgnoreCase)).Any()))
            {
                System.Reflection.PropertyInfo npi = newTpis.Where(np => string.Equals(np.Name, fi.Name, StringComparison.OrdinalIgnoreCase)).First();
                npi.SetValue(newT, fi.GetValue(source), null);
            }
            foreach (System.Reflection.FieldInfo fi in source.GetType()
               .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField)
               .Where(p => newTfis.Where(np => string.Equals(np.Name, p.Name, StringComparison.OrdinalIgnoreCase)).Any()))
            {
                System.Reflection.FieldInfo npi = newTfis.Where(np => string.Equals(np.Name, fi.Name, StringComparison.OrdinalIgnoreCase)).First();
                npi.SetValue(newT, fi.GetValue(source));
            }
            return newT;
        }

    }
}
