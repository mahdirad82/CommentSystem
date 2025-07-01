using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CommentSystem.Api;

public class AddCustomHeadersOperationFilter : IOperationFilter
{
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