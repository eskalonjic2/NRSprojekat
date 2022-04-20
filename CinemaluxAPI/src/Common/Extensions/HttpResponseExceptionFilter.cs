using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CinemaluxAPI.Common.Extensions
{
    //TODO Make this better
    public class HttpResponseException : Exception
    {
        public HttpResponseException(HttpStatusCode status, object value)
        {
            StatusCode = (int) status;
            Value = value;
        }
        
        public int StatusCode { get; set; } = 500;

        public object Value { get; set; }
    }
    
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is HttpResponseException exception)
            {
                context.Result = new ObjectResult(exception.Value)
                {
                    StatusCode = exception.StatusCode,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}