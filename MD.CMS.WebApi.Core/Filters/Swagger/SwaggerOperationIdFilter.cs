using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MD.CMS.WebApi.Core.Filters.Swagger
{
    public class SwaggerOperationIdFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            string controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ApiDescription.ActionDescriptor).ControllerName;
            string actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ApiDescription.ActionDescriptor).ActionName;

            operation.OperationId = $"{controllerName}_{context.ApiDescription.HttpMethod}_{actionName}";
        }
    }
}
