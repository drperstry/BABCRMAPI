using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BabCrm.ApiGateway.Filters
{
    public class HeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            // Add the language header parameter
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Required = false, // Set to true if language is mandatory
                Description = "Language preference header (e.g., En, Ar)"
            });

            // add the message id header parameter
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Message-ID",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Required = false, // Set to true if language is mandatory
                Description = "Message id of the request"
            });
        }
    }
}
