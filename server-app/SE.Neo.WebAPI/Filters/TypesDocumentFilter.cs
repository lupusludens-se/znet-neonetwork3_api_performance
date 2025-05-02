using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SE.Neo.WebAPI.Filters
{
    public class TypesDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (OpenApiPathItem? path in swaggerDoc.Paths.Values)
                foreach (OpenApiOperation? operation in path.Operations.Values)
                    foreach (OpenApiParameter? parameter in operation.Parameters)
                    {
                        string? schemaReferenceId = parameter.Schema.Reference?.Id;
                        if (string.IsNullOrEmpty(schemaReferenceId))
                            continue;

                        OpenApiSchema schema = context.SchemaRepository.Schemas[schemaReferenceId];

                        int cutStart = schema.Description.IndexOf("<ul>");
                        int cutEnd = schema.Description.IndexOf("</ul>") + 5;

                        parameter.Description += schema.Description
                            .Substring(cutStart, cutEnd - cutStart);
                    }
        }
    }
}
