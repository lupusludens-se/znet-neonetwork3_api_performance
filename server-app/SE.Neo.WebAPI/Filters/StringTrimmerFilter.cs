using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace SE.Neo.WebAPI.Filters
{
    public class StringTrimmerFilter : ActionFilterAttribute
    {
        private readonly ILogger<StringTrimmerFilter> _logger;

        public StringTrimmerFilter(ILogger<StringTrimmerFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            foreach (string key in actionContext.ActionArguments.Keys)
            {
                var item = actionContext.ActionArguments[key];
                if (item != null)
                {
                    if (item.GetType() == typeof(string))
                    {
                        actionContext.ActionArguments[key] = ((string)item).Trim();
                    }
                    else if (item.GetType().Name == typeof(List<>).Name)
                    {
                        var itemEnumerable = item as IEnumerable<object>;
                        if (itemEnumerable != null)
                        {
                            foreach (var value in itemEnumerable)
                            {
                                ProcessItemValues(value);
                            }
                        }
                    }
                    else
                    {
                        ProcessItemValues(item);
                    }
                }
            }
        }

        private void ProcessItemValues(object? item, in int level = 0)
        {
            const int maxLevel = 5;
            if (level > maxLevel)
                return;
            try
            {
                Type type = item.GetType();
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    var propValue = propertyInfo.GetValue(item);
                    if (propValue != null && propValue.GetType().Name == typeof(List<>).Name)
                    {
                        var propValueEnumerable = propValue as IEnumerable<object>;
                        if (propValueEnumerable != null)
                        {
                            foreach (var value in propValueEnumerable)
                            {
                                ProcessItemValues(value, level + 1);
                            }
                        }
                    }
                    else if (propValue != null && propValue.GetType() == typeof(string))
                    {
                        propertyInfo.SetValue(item, ((string)propValue).Trim());
                    }
                    else if (propValue != null && propValue.GetType().IsClass && propValue.GetType() != typeof(FormFile))
                    {
                        ProcessItemValues(propValue, level + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error with string trimmer.");
            }
        }
    }
}