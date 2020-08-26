using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ErrorHandler
{
    public static class ErrorHandlerExtension
    {
        public static void UseExceptionFilter(this MvcOptions options)
        {
            options.Filters.Add(new HttpResponseExceptionFilter());
        }
    }

    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is BusinessException exception)
            {
                context.Result = new ObjectResult(new
                {
                    errorCode = exception.Code
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
