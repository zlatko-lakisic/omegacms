using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.WebApi.Core.Properties;
using MD.Tools.Helpers.Core.TypeConversion;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using MD.CMS.BusinessLogic.WebApi.Core.Models;
using Microsoft.AspNetCore.Builder;
using MD.CMS.BusinessLogic.WebApi.Core.Extensions;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Options;

namespace MD.CMS.BusinessLogic.WebApi.Core.Modules
{
    public class EntityAliasMiddleware
    {
        private readonly RequestDelegate _next;

        public EntityAliasMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await context_BeginRequest(context);

            await _next.Invoke(context);
        }

        async Task context_BeginRequest(HttpContext context)
        {
            if (context.Request.PathBase.Value.ToLowerInvariant().StartsWith(string.Format("{0}{1}", PathString.FromUriComponent("~/"), Settings.Default.EntityAliasBase.ToLowerInvariant())))
            {
                context.Response.ContentType = "application/json";
                string redirectUrl = context.Request.PathBase.Value.Replace(string.Format("{0}{1}/", PathString.FromUriComponent("~/"), Settings.Default.EntityAliasBase), string.Empty);
                if (redirectUrl.StartsWith("/"))
                {
                    redirectUrl = redirectUrl.Substring(1);
                }
                string[] urlParts = redirectUrl.Split('/');
                string entityName = string.Empty;
                string methodName = string.Empty;
                List<string> parameters = new List<string>();
                for (int i = 0; i < urlParts.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            entityName = urlParts[i];
                            break;
                        case 1:
                            methodName = urlParts[i];
                            break;
                        default:
                            parameters.Add(urlParts[i]);
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(entityName) && !string.IsNullOrEmpty(methodName))
                {
                    ContentTypeDefinition<ContentTypeDefinitionField> definition = (await ContentTypeDefinitionController.GetNewInstance().GetAllAsync<ContentTypeDefinitionField>()).FirstOrDefault(ct => string.Compare(ct.Name, entityName, true).Equals(0));
                    if (definition != null)
                    {
                        string lcidString = context.Request.Headers.GetValue(Settings.Default.LCIDHeaderName);
                        int lcid = default(int);
                        if (!string.IsNullOrEmpty(lcidString) && (await CultureController.GetNewInstance().GetAllAsync()).Any(c => c.LCID == lcidString.ToInt32(default(int))))
                        {
                            lcid = lcidString.ToInt32(default(int));
                        }
                        switch (methodName.ToLowerInvariant())
                        {
                            case "getbyid":
                                await HandleGetById(context, definition, parameters.First(), lcid);
                                break;
                        }
                    }
                }
                throw new HttpException(404, "Endpoint not found!");
            }
        }

        private async Task HandleGetById(HttpContext context, ContentTypeDefinition<ContentTypeDefinitionField> definition, string id, int lcid)
        {
            Content content = (await ContentController<Content>.GetNewInstance().GetByIdAsync(new ContentOptions
            {
                ContentIds = new List<string> { id },
                Lcid = lcid,
                FillFields = true,
                FillMetaData = true
            })).FirstOrDefault();
            if (content != null && content.ContentTypeDefinitionId.Equals(definition.Id))
            {
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
                        sw.Write(ToJson(content));
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
                    await context.Response.WriteAsync(responseContent);
                }
            }
        }

        private bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        private string ToJson(Content content)
        {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            fields.Add("Id", content.Id);
            fields.MergeFrom<string, object>(content.ContentType.Fields.ToDictionary(field => field.Name, field => field.Value as object));
            fields.MergeFrom<string, object>(content.MetaDataFieldValues.ToDictionary(field => field.Name, field => field.Value as object));
            return ToJson(fields);
        }

        private string ToJson(Dictionary<string, object> fields)
        {
            fields = fields.Where(field => !string.IsNullOrEmpty(field.Key) && field.Value != null).ToDictionary(field => field.Key, field => field.Value);
            StringBuilder response = new StringBuilder("{");
            foreach (KeyValuePair<string, object> field in fields)
            {
                string value = field.Value.ToString();
                bool isString = !IsNumeric(field.Value);
                if (isString)
                {
                    bool testBool = false;
                    isString = !bool.TryParse(field.Value.ToString(), out testBool);
                }

                if (isString)
                {
                    value = HttpUtility.JavaScriptStringEncode(field.Value.ToString());
                }
                response.Append(string.Format(isString ? "\"{0}\": \"{1}\"," : "\"{0}\": {1},", HttpUtility.JavaScriptStringEncode(field.Key).Replace(" ", "_"), value));
            }
            response.Remove(response.Length - 1, 1);
            response.Append("}");
            return response.ToString();
        }
    }

    public static class EntityAliasMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EntityAliasMiddleware>();
        }
    }
}