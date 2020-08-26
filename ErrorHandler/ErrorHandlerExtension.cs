using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace ErrorHandler
{
    public static class ErrorHandlerExtension
    {
        public static void UseExceptionFilter(this MvcOptions options)
        {
            options.Filters.Add<HttpResponseExceptionFilter>();
        }

        public static void AddLocalization(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
        }

        public static void UseLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = new[] { "en-US", "zh-CN" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);
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
                    message = _localizer[((int)exception.Code).ToString()]
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
