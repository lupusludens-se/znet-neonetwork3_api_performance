using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using SE.Neo.WebAPI.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace SE.Neo.WebAPI.Filters
{
    public class CustomEnvironmentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (ApiDescription description in context.ApiDescriptions)
            {
                string path = description.RelativePath!;

                description.TryGetMethodInfo(out MethodInfo info);
                IEnumerable<DevTestApiAttribute> devTestAttributes = info.GetCustomAttributes(true).OfType<DevTestApiAttribute>().Distinct();

                if (Environment.GetEnvironmentVariable("APP_ENV") == "prod")
                {
                    if (devTestAttributes.Any())
                    {
                        RemoveOtherRoutes(swaggerDoc, path);
                    }
                }
            }
        }

        private static void RemoveOtherRoutes(OpenApiDocument swaggerDoc, string path)
        {
            var removeRoutes = swaggerDoc.Paths
                .Where(x => x.Key.ToLower().Contains(path.ToLower()))
                .ToList();

            removeRoutes.ForEach(x => { swaggerDoc.Paths.Remove(x.Key); });
        }
    }
}
