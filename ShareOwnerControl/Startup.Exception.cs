using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ShareOwnerControl.Models;

namespace ShareOwnerControl
{
    public partial class Startup
    {

        public void ConfigureException(IServiceCollection services)
        {
            services.AddMvc(options => { options.Filters.Add<ExceptionHandlerFilterAttribute>(); });
        }

        public class ExceptionHandlerFilterAttribute : ExceptionFilterAttribute
        {
            public ExceptionHandlerFilterAttribute()
            {

            }

            public override void OnException(ExceptionContext context)
            {
                var exception = context.Exception;

                context.HttpContext.Response.StatusCode = 500;
                switch (exception)
                {
                    case null:
                        // Should never happen
                        return;
                    case ApiException apiException:
                        context.Result = new JsonResult(new ApiError(apiException.Message));
                        break;
                    default:
                        context.Result = new JsonResult(new ApiError("Unhandled exception"));
                        break;
                }
                base.OnException(context);
            }

        }

        public class ApiError
        {
            public string Message { get; set; }
            public ApiError(string message)
            {
                Message = message;
            }
        }
    }
}