using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Reflection;

namespace SE.Neo.WebAPI.Filters
{
    public class DecodeFilter : ActionFilterAttribute
    {
        private readonly ILogger<DecodeFilter> _logger;

        private readonly string[] ForbiddenSymbols = { "+" };

        public DecodeFilter(ILogger<DecodeFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            if (actionContext.HttpContext.Request.Method.ToString() == WebRequestMethods.Http.Get ||
                actionContext.HttpContext.Request.Method.ToString() == WebRequestMethods.Http.Post ||
                actionContext.HttpContext.Request.Method.ToString() == WebRequestMethods.Http.Put)
            {
                foreach (string key in actionContext.ActionArguments.Keys)
                {
                    var item = actionContext.ActionArguments[key];
                    if (item != null)
                    {
                        if (item.GetType() == typeof(string))
                        {
                            if (ContainsHtml((string)item))
                            {
                                actionContext.ActionArguments[key] = DecodeTags((string)item);
                            }
                            else if (ContainsUrl((string)item))
                            {
                                actionContext.ActionArguments[key] = WebUtility.UrlDecode((string)item);
                            }
                        }
                        else if (item.GetType().Name == typeof(List<>).Name)
                        {
                            foreach (var value in item as IEnumerable<object>)
                            {
                                ProcessItemValues(value);
                            }
                        }
                        else
                        {
                            ProcessItemValues(item);
                        }
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
                        if (ContainsHtml((string)propValue))
                        {
                            propertyInfo.SetValue(item, DecodeTags((string)propValue));
                        }
                        else if (ContainsUrl((string)propValue) || ContainsForbiddenSymbols((string)propValue))
                        {
                            propertyInfo.SetValue(item, WebUtility.UrlDecode((string)propValue));
                        }
                    }
                    else if (propValue != null && propValue.GetType().IsClass && propValue.GetType() != typeof(FormFile))
                    {
                        ProcessItemValues(propValue, level + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error with Url Decode.");
            }
        }

        private bool ContainsHtml(string text)
        {
            return text != null &&
                    (text.Contains(WebUtility.UrlEncode("&lt;")) ||
                     text.Contains(WebUtility.UrlEncode("&gt;")) ||
                     text.Contains(WebUtility.UrlEncode("&amp;")));
        }

        private bool ContainsUrl(string text)
        {
            return text != null &&
                    (text.Contains("http") ||
                     text.Contains("ftp") ||
                     text.Contains("www."));
        }

        private bool ContainsForbiddenSymbols(string text)
        {
            return text != null && ForbiddenSymbols.Any(fs => text.Contains(WebUtility.UrlEncode(fs)));
        }

        private string DecodeTags(string text)
        {
            return WebUtility.HtmlDecode(WebUtility.UrlDecode(text));
        }
    }
}