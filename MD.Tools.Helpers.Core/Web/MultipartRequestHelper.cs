using Microsoft.Net.Http.Headers;
using System;
using System.IO;

namespace MD.Tools.Helpers.Core.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class MultipartRequestHelper
    {
        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec says 70 characters is a reasonable limit.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="lengthLimit"></param>
        /// <returns></returns>
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
            if (string.IsNullOrWhiteSpace(boundary.Value))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.Contains("multipart/", StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentDisposition"></param>
        /// <returns></returns>
        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data", StringComparison.OrdinalIgnoreCase)
                   && string.IsNullOrEmpty(contentDisposition.FileName.Value)
                   && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentDisposition"></param>
        /// <returns></returns>
        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data", StringComparison.OrdinalIgnoreCase)
                   && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                       || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }
    }
}
