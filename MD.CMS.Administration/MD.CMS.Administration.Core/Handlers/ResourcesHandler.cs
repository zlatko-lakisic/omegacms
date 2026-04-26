using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Globalization;
using System.Resources;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using System;

namespace MD.CMS.Administration.Core.Handlers
{
    public class ResourcesHandler
    {
        private readonly RequestDelegate _next;
        private IHttpResponseStreamWriterFactory _streamWriterFactory;

        public ResourcesHandler(RequestDelegate next, IHttpResponseStreamWriterFactory streamWriterFactory)
        {
            _next = next;
            _streamWriterFactory = streamWriterFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsResourceRequest(context))
            {
                await ProcessRequest(context);
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        private bool IsResourceRequest(HttpContext context)
        {
            return context.Request.Path.ToString().Contains("assets/resources.json");
        }

        private async Task ProcessRequest(HttpContext context)
        {
            string cultureName = context.Request.Query["cultureName"];
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(GetResources(cultureName)));
        }

        public static string GetResources(string cultureName)
        {
            List<string> jsonStrings = new List<string>();
            foreach (KeyValuePair<string, ResourceManager> resource in MD.CMS.BusinessLogic.Administration.Core.Resources.ResourceManager.Loadedresources)
            {
                jsonStrings.Add(GetResourceFileJson(resource.Key, resource.Value, cultureName));
            }
            return "{" + string.Join(",", jsonStrings) + "}";
        }

        private static string GetResourceFileJson(string resourceFileName, ResourceManager resourceManager, string cultureName)
        {
            try
            {
                ResourceSet resourceSet = resourceManager.GetResourceSet(CultureInfo.GetCultureInfo(cultureName), true, true);
                return "\"" + resourceFileName + "\" : " + JsonConvert.SerializeObject(resourceSet.Cast<DictionaryEntry>().ToDictionary(x => x.Key.ToString().Replace("_", "-"), x => x.Value.ToString()));
            }
            catch(Exception error)
            {

            }
            return string.Empty;
        }
    }
}
