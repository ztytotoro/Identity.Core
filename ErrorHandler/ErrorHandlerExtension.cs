using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Primitives;
using System;
using System.Globalization;
using System.Linq;

namespace ErrorHandler
{
    public static class ErrorHandlerExtension
    {
        public static void UseExceptionFilter(this MvcOptions options)
        {
            options.Filters.Add<HttpResponseExceptionFilter>();
        }

        public static void AddErrorLocalization(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
        }

        public static void UseErrorLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = new[] { "en-US", "zh-CN" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            // Custom language process
            app.Use(async (context, next) => {
                context.Request.Query.TryGetValue("lang", out StringValues queryLanguages);
                context.Request.Headers.TryGetValue("language", out StringValues languages);

                string language = null;

                if (queryLanguages.Count > 0)
                {
                    language = queryLanguages.Last();
                }
                else if (languages.Count > 0)
                {
                    language = languages.Last();
                }

                if(!string.IsNullOrEmpty(language))
                {
                    var culture = localizationOptions.SupportedCultures.FirstOrDefault(
                        supportedCulture => StringSegment.Equals(supportedCulture.Name, language, StringComparison.OrdinalIgnoreCase));
                    if (culture != null)
                    {
                        CultureInfo.CurrentCulture = culture;
                        CultureInfo.CurrentUICulture = culture;
                    }
                }

                await next.Invoke();
            });
        }
    }

    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        private readonly IStringLocalizer<HttpResponseExceptionFilter> _localizer;

        public HttpResponseExceptionFilter(IStringLocalizer<HttpResponseExceptionFilter> localizer) {
            _localizer = localizer;
        }

        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is BusinessException exception)
            {
                context.Result = new ObjectResult(new
                {
                    errorCode = exception.Code,
                    message = _localizer[((int)exception.Code).ToString()].Value
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
