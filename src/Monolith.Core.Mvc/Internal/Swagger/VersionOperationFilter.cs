using System;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Monolith.Core.Mvc.Internal.Swagger
{
    internal class VersionOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var version = operation.Parameters.Single(parameter =>
                parameter.Name.Equals("version", StringComparison.InvariantCultureIgnoreCase));

            operation.Parameters.Remove(version);
        }
    }
}
