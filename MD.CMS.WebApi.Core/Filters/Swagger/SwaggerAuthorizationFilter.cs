using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MD.CMS.WebApi.Core.Filters.Swagger
{
    public class SwaggerAuthorizationFilter : IOperationFilter
    {
        public static string Token { get; set; }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            OpenApiParameter parameter = new OpenApiParameter
            {
                Name = MD.CMS.BusinessLogic.WebApi.Core.Properties.Settings.Default.AuthenticateHeaderName,
                In = ParameterLocation.Header,
                Description = "Access Token",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum = (new string[] { }).Select(val => (IOpenApiAny)new OpenApiString(val)).ToList(),
                    Default = new OpenApiString(Token)
                },
                AllowReserved = true
            };

            operation.Parameters.Add(parameter);
        }
    }
}
