using MD.CMS.BusinessLogic.WebApi.Core.Modeles;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MD.Tools.Helpers.Core.Extensions.StringExt;
using System.Net;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modules
{
    public class SecurityApiModule
    {
        string key = "I/YGVv0Toc81seeRd+CipEsNGFXQhaCb1HVIlkKd8vY=";
        string iv = "OytxFiJFA6PzjbaovbzaDg==";
        private HttpContext _context;

        private readonly RequestDelegate _next;

        public SecurityApiModule(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            _context = context;
            SecureApi_BeginRequest(context);

            await _next.Invoke(context);
        }

        public void SecureApi_BeginRequest(HttpContext context)
        {

            if (!string.IsNullOrEmpty(context.Request.Headers["IsSecureApi"].FirstOrDefault()) && string.Compare(context.Request.Method, "post", true).Equals(0) && context.Request.Path.Value.ToLowerInvariant().Contains("SecureApi/MessageHandler".ToLowerInvariant()))
            {
                string encryptedJson = GetStreamBody(context.Request.Body);
                CryptedMessageModel cryptedMessage = new CryptedMessageModel() { data = encryptedJson };
                string decryptedJson = Decrypt(cryptedMessage.data, key, iv);
                SecureMessageModel message = JsonConvert.DeserializeObject<SecureMessageModel>(decryptedJson);

                HttpWebResponse response = GetResponse(context, message);

                string textResponse = string.Empty;

                using(StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    textResponse = reader.ReadToEnd();
                }

                context.Response.ContentType = "application/json";
                context.Response.Clear();
                context.Response.StatusCode = response.StatusCode.GetIntValue(200);
                using (MemoryStream customStream = new MemoryStream())
                {
                    // Create a backup of the original response stream
                    var backup = context.Response.Body;

                    // Assign readable/writeable stream
                    context.Response.Body = customStream;

                    // Restore the response stream
                    context.Response.Body = backup;

                    StreamWriter sw = new StreamWriter(customStream, new UnicodeEncoding());
                    try
                    {
                        sw.Write(textResponse);
                        sw.Flush();//otherwise you are risking empty stream
                        customStream.Seek(0, SeekOrigin.Begin);

                        // Test and work with the stream here. 
                        // If you need to start back at the beginning, be sure to Seek again.
                    }
                    finally
                    {
                        sw.Dispose();
                    }

                    // Move to start and read response content
                    customStream.Seek(0, SeekOrigin.Begin);
                    var responseContent = new StreamReader(customStream).ReadToEnd();

                    // Write custom content to response
                    context.Response.WriteAsync(responseContent);
                }


                /*context.Response.Write(BuildSecureResponse(textResponse, message));
                context.Response.End();*/
            }
        }

        private string BuildSecureResponse(string content, SecureMessageModel secureRequest)
        {
            SecureMessageModel message = secureRequest;
            message.message.data = content;

            _context.Request.Headers.Remove("IsSecureApi");

            CryptedMessageModel cryptedMessage = new CryptedMessageModel();
            cryptedMessage.data = Encrypt(JsonConvert.SerializeObject(message), key, iv);

            return JsonConvert.SerializeObject(cryptedMessage);
        }

        private HttpWebResponse GetResponse(HttpContext context, SecureMessageModel message)
        {
            string url = string.Format("{0}{1}", context.Request.Path.Value, message.endpoint);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = false;

            if (message.message.isJsonArray)
            {
                request.ContentType = "application/json; charset=UTF-8";
            }
            else
            {
                request.ContentType = "application/json";
            }

            message.message.headers.Add(new SecureMessageModel.SecureMessageModel_Message.SecureMessageModel_MessageHeader() { name = "IsSecureApi", value = "true" });

            foreach (SecureMessageModel.SecureMessageModel_Message.SecureMessageModel_MessageHeader header in message.message.headers)
            {
                if (!string.IsNullOrEmpty(header.name) && !string.IsNullOrEmpty(header.value))
                {
                    if (request.Headers[header.name] != null)
                    {
                        request.Headers[header.name] = header.value;
                    }
                    else
                    {
                        request.Headers.Add(header.name, header.value);
                    }
                }
            }

            switch (message.message.method.ToLowerInvariant())
            {
                case "get":
                    request.Method = "GET";
                    break;
                case "post":
                    request.Method = "POST";
                    request.ContentLength = message.message.data.ToSafeString().Length;
                    request.GetRequestStream().Write(UTF8Encoding.UTF8.GetBytes(message.message.data.ToSafeString()), 0, message.message.data.ToSafeString().Length);
                    break;
                case "delete":
                    request.Method = "DELETE";
                    break;
            }

            return (HttpWebResponse)request.GetResponse();
        }


        private string Decrypt(string ciphertext, string key, string iv)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();
            using (ICryptoTransform decryptor = aes.CreateDecryptor(Convert.FromBase64String(key), Convert.FromBase64String(iv)))
            {
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);
                byte[] bytes = Convert.FromBase64String(ciphertext);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
                ms.Position = 0;
                bytes = new byte[ms.Length];
                ms.Read(bytes, 0, bytes.Length);
                return utf8.GetString(bytes);
            }
        }

        private string Encrypt(string plaintext, string key, string iv)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();
            using (ICryptoTransform encryptor = aes.CreateEncryptor(Convert.FromBase64String(key), Convert.FromBase64String(iv)))
            {
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                byte[] bytes = utf8.GetBytes(plaintext);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
                ms.Position = 0;
                bytes = new byte[ms.Length];
                ms.Read(bytes, 0, bytes.Length);
                return Convert.ToBase64String(bytes);
            }
        }

        private string GetStreamBody(Stream stream)
        {
            StreamReader bodyStream = new StreamReader(stream);
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            return bodyText;
        }
    }
}