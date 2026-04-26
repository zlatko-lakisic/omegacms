using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace MD.Tools.Helpers.Core.Web.Formatters
{
    /// <summary>
    /// 
    /// </summary>
    public class JavaScriptFormatter : TextOutputFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; }
        /// <summary>
        /// 
        /// </summary>
        public JavaScriptFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/javascript"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        // optional, but makes sense to restrict to a specific condition
        protected override bool CanWriteType(Type type)
        {
            if (typeof(string).IsAssignableFrom(type)
                || typeof(IEnumerable<string>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="selectedEncoding"></param>
        /// <returns></returns>
        // this needs to be overwritten
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var serviceProvider = context.HttpContext.RequestServices;

            var response = context.HttpContext.Response;

            var buffer = new StringBuilder();
            if (context.Object is IEnumerable<string>)
            {
                foreach (var script in context.Object as IEnumerable<string>)
                {
                    buffer.AppendLine(script);
                }
            }
            else
            {
                var script = context.Object as string;
                buffer.AppendLine(script);
            }
            return response.WriteAsync(buffer.ToString());
        }
    }
}
