using SE.Neo.EmailTemplates.Models.BaseModel;
using ActionContext = Microsoft.AspNetCore.Mvc.ActionContext;
namespace SE.Neo.Infrastructure.Services.Interfaces
{
    public interface IRenderEmailTemplateService
    {
        Task<string> RenderEmailTemplateAsync<TModel>(TModel model, ActionContext context) where TModel : BaseTemplatedEmailModel;

    }
}
