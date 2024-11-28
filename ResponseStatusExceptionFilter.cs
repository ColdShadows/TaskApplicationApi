using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using TaskApplicationApi.Exceptions;

namespace TaskApplicationApi
{
    public class ResponseStatusExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status;

            switch (context.Exception)
            {
                case ResourceNotFoundException:
                    status = HttpStatusCode.NotFound;
                    break;
                case ResourceAlreadyExistsException:
                    status = HttpStatusCode.Conflict;
                    break;
                case ResourceForbiddenException:
                    status = HttpStatusCode.Forbidden;
                    break;
                default:
                    status = HttpStatusCode.InternalServerError;
                    break;
            }

            //You can enable logging error

            context.ExceptionHandled = true;
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
        }
    }
}
