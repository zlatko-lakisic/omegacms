using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Globalization;
using System.IO;

namespace MD.Tools.Helpers.Core.TypeConversion
{
    public static partial class Utility
    {

        /// <summary>
        /// Toes the readable string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string ToReadableString(this Type type)
        {
            return ToReadableString(type, false);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="useFullNames">if set to <c>true</c> [use full names].</param>
        /// <returns></returns>
        public static string ToReadableString(this Type type, bool useFullNames)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            string value = null;
            if (type.IsGenericType)
            {
                string cacheKey = "TypeToString_{0}_{1}".ToFormattedString(type.GetHashCode(), useFullNames);
                if (string.IsNullOrEmpty(value))
                {
                    using (CodeDomProvider csharpProvider = CodeDomProvider.CreateProvider("C#"))
                    {
                        CodeTypeReference typeReference = new CodeTypeReference(type);
                        CodeTypeReferenceExpression variableDeclaration = new CodeTypeReferenceExpression(typeReference);
                        StringBuilder sb = new StringBuilder();
                        using (StringWriter writer = new StringWriter(sb, CultureInfo.InvariantCulture))
                        {
                            csharpProvider.GenerateCodeFromExpression(variableDeclaration, writer, new CodeGeneratorOptions());
                        }

                        value = (useFullNames) ? AbbreviateGenericTypeName(sb.ToString()) : sb.ToString();
                    }
                }
            }
            else
            {
                value = (useFullNames) ? type.FullName : type.Name;
            }
            return value;

        }

        private static System.Text.RegularExpressions.Regex _abbreviate = new System.Text.RegularExpressions.Regex(@".*?\.?([^\.\<\>\,]+)([\<\>\,]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        private static string AbbreviateGenericTypeName(string name)
        {
            if (!_abbreviate.IsMatch(name)) return name;
            return _abbreviate.Matches(name).OfType<System.Text.RegularExpressions.Match>()
                .Select(m => string.Concat(m.Groups[1].Value, m.Groups[2].Value))
                .ToDelimitedString(string.Empty);
        }

        /// <summary>
        /// Parses the type name
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static Type ToType(this string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentOutOfRangeException(nameof(typeName));
            }

            try
            {
                int openPos = typeName.IndexOf('<', StringComparison.OrdinalIgnoreCase);
                int closePos = typeName.LastIndexOf('>');
                if (openPos > -1 && closePos > openPos)
                {
                    Type[] parameterTypes = GetParameterTypes(typeName, openPos, closePos);
                    string t = string.Format(CultureInfo.InvariantCulture, "{0}`{1}{2}", typeName.Substring(0, openPos), parameterTypes.Length, typeName.Substring(closePos + 1));
                    Type generic = Type.GetType(t);
                    if (generic == null) throw new TypeLoadException(PrepareTypeLoadMessage(t));
                    return generic.MakeGenericType(parameterTypes);
                }

                return Type.GetType(typeName);

            }
            catch (NotSupportedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(PrepareTypeLoadMessage(typeName), ex);
            }
        }

        private static string PrepareTypeLoadMessage(string typeName)
        {
            return "Cannot Create Type of '{0}'.  Do you need the assembly reference? \n For Example:\n Website.Support.PageTypes.BasicContentPage<Website.Support.Content.Pages.ContentPage, Website.Support>, Website.Support\n\nWhere Website.Support is the assembly defining the types.\n\n".ToFormattedString(typeName);
        }

        private static Type[] GetParameterTypes(string typeName, int openPos, int closePos)
        {
            string[] typeNames = typeName.Substring(openPos + 1, closePos - openPos - 1)
                .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Type[] types = typeNames
                .Select(p => p.ToType())
                .Where(t => t != null).ToArray();
            if (types == null || !types.Any())
            {
                List<Type> typeList = new List<Type>();
                for (int i = 0; i < typeNames.Length; i += 2)
                {
                    typeList.Add(string.Concat(typeNames[i], ", ", typeNames[i + 1]).ToType());
                }
                types = typeList.Where(t => t != null).ToArray();
            }
            return types;
        }


    }
}
