using GlobalRoutes.SharedKernel.Properties;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GlobalRoutes.Api.Filters
{
    public class LanguageFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var currentEndpoint = context.ApiDescription.RelativePath;
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = NameStrings.HeaderName_Language,
                In = ParameterLocation.Header,
                Required = false,
                Description = NameStrings.HeaderName_Language,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("")
                }
            });
        }
    }
}
