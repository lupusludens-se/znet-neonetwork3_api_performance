using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Xml.Linq;

namespace SE.Neo.WebAPI.Filters
{
    public class TypesSchemaFilter : ISchemaFilter
    {
        private readonly XDocument _xmlComments;

        public TypesSchemaFilter(string xmlPath)
        {
            if (File.Exists(xmlPath))
            {
                _xmlComments = XDocument.Load(xmlPath);
            }
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (_xmlComments == null)
                return;

            if (schema.Enum != null && schema.Enum.Count > 0 && context.Type != null && context.Type.IsEnum)
            {
                schema.Description += "<p>Members:</p><ul>";

                string? fullTypeName = context.Type.FullName;

                foreach (var enumMemberName in schema.Enum.OfType<OpenApiString>().Select(v => v.Value))
                {
                    string? fullEnumMemberName = $"F:{fullTypeName}.{enumMemberName}";

                    XElement? enumMemberComments = _xmlComments.Descendants("member")
                        .FirstOrDefault(m => m.Attribute("name")?.Value.Equals(fullEnumMemberName, StringComparison.OrdinalIgnoreCase) ?? false);
                    if (enumMemberComments == null)
                        continue;

                    XElement? summary = enumMemberComments.Descendants("summary").FirstOrDefault();
                    if (summary == null)
                        continue;

                    schema.Description += $"<li><i>{enumMemberName}</i> - {summary.Value.Trim()}</ li > ";
                }

                schema.Description += "</ul>";
            }
        }
    }
}
