using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Xml.Linq;

namespace SE.Neo.WebAPI.Filters
{
    public class EnumTypesSchemaFilter : ISchemaFilter
    {
        private readonly XDocument _xmlComments;

        public EnumTypesSchemaFilter(string xmlPath)
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
                string? fullTypeName = context.Type.FullName;

                schema.Description = $"<p>{fullTypeName}:</p><ul>";

                Array enumValues = Enum.GetValues(context.Type);
                string[] enumName = Enum.GetNames(context.Type);
                Type underlyingEnumType = Enum.GetUnderlyingType(context.Type);
                for (int index = 0; index < enumName.Length; ++index)
                    schema.Description += $"<li><i>{enumName[index]}</i> - {Convert.ChangeType(enumValues.GetValue(index), underlyingEnumType)}</ li > ";
                schema.Description += "</ul>";
            }
        }
    }
}
