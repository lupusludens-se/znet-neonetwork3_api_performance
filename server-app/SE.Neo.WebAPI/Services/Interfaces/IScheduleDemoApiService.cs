using Microsoft.AspNetCore.Mvc;
using SE.Neo.WebAPI.Models.ScheduleDemo;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IScheduleDemoApiService
    {
        Task SendScheduleDemoMessageAsync(ScheduleDemoRequest model, ActionContext context);
    }
}