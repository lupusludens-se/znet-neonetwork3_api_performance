using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SE.Neo.EmailTemplates.Models.BaseModel;
using SE.Neo.Infrastructure.Services.Interfaces;
using ActionContext = Microsoft.AspNetCore.Mvc.ActionContext;
namespace SE.Neo.Infrastructure.Services
{
    public class RenderEmailTemplateService : IRenderEmailTemplateService
    {
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;

        public RenderEmailTemplateService(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderEmailTemplateAsync<TModel>(TModel model, ActionContext context) where TModel : BaseTemplatedEmailModel
        {
            ViewEngineResult viewResult = _viewEngine.FindView(context, model.TemplateName, false);
            if (!viewResult.Success)
            {
                string searchedLocations = string.Join(',', viewResult.SearchedLocations);
                throw new Exception($"Could not find {model.TemplateName}, searched {searchedLocations}");
            }

            IView view = viewResult.View;
            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    context,
                    view,
                    new ViewDataDictionary<TModel>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        context.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions());
                await view.RenderAsync(viewContext);
                return output.ToString();
            }
        }
    }
}
