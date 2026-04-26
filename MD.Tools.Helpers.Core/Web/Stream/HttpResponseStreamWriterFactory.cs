using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.IO;
using System.Text;

namespace MD.Tools.Helpers.Core.Web.Stream
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpResponseStreamWriterFactory : IHttpResponseStreamWriterFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public TextWriter CreateWriter(System.IO.Stream stream, Encoding encoding)
        {
            return new StreamWriter(stream, encoding);
        }
    }
}
