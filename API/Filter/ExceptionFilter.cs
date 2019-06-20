using System.Net;
using System.Security.Authentication;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace API.Filter
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled && context.Exception.GetType() == typeof(InvalidCredentialException))
            {
                var json = JsonConvert.SerializeObject(new
                {
                    message = context.Exception.Message
                });
                var messageBytes = Encoding.ASCII.GetBytes(json);

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.HttpContext.Response.ContentLength = messageBytes.Length;
                context.HttpContext.Response.ContentType = "application/json; charset=utf-8";
                context.HttpContext.Response.Body.WriteAsync(messageBytes);
                context.ExceptionHandled = true;
            }
        }
    }
}
