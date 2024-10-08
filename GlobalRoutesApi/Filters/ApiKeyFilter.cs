using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using GlobalRoutes.SharedKernel.Properties;

namespace GlobalRoutes.Api.Filters
{
    public class ApiKeyFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = NameStrings.HeaderName_ApiKey,
                In = ParameterLocation.Header,
                Required = true,
                Description = "X-Api-Key",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("")
                }
            });
        }
    }
}
