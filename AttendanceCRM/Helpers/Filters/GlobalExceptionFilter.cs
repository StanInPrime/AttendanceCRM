using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AttendanceCRM.Helpers.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter, IDisposable
    {
        public GlobalExceptionFilter()
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OnException(ExceptionContext exceptionContext)
        {
            Logger.Logger.WriteErrorFile(exceptionContext.Exception);

            //If the request is AJAX return JSON else redirect user to Error view.
            if (exceptionContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                exceptionContext.Result = new JsonResult(new { error = exceptionContext.Exception.Message }, new Newtonsoft.Json.JsonSerializerSettings());
            }
            else
            {
                exceptionContext.ExceptionHandled = true;
                exceptionContext.Result = new RedirectToActionResult("error", "account", new { });
            }
        }
    }
}
