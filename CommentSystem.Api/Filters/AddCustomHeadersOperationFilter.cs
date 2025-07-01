using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CommentSystem.Api.Filters;

/// <summary>
/// Adds custom headers (UserId, Role) to Swagger operations for authentication/authorization.
/// </summary>
public class AddCustomHeadersOperationFilter : IOperationFilter
{
    /// <summary>
    /// Applies the filter to the specified operation.
    /// </summary>
    /// <param name="operation">The OpenAPI operation.</param>
    /// <param name="context">The operation filter context.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "UserId",
            In = ParameterLocation.Header,
            Description = "User ID for authentication",
            Required = false,
            Schema = new OpenApiSchema { Type = "int" }
        });

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Role",
            In = ParameterLocation.Header,
            Description = "User Role for authorization (e.g., Admin, User)",
            Required = false,
            Schema = new OpenApiSchema { Type = "string" }
        });
    }
}