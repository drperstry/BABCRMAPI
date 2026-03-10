using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BabCrm.ApiGateway.Filters
{
    public class ExcludeControllerFilter<T> : IDocumentFilter where T : ControllerBase
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var excludedControllerName = typeof(T).Name.Replace("Controller", string.Empty);
            var pathsToRemove = swaggerDoc.Paths
                .Where(path => path.Key.Contains(excludedControllerName))
                .ToList();

            foreach (var path in pathsToRemove)
            {
                swaggerDoc.Paths.Remove(path.Key);
            }
        }
    }
}
