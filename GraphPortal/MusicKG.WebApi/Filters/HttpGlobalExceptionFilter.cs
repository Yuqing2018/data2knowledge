using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MusicKG.Service.Helpers;
using MusicKG.WebApi.Contract.ViewModels;
using System;
using System.Net;

namespace MusicKG.WebApi.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger logger;

        public HttpGlobalExceptionFilter(
            ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var data = context.Exception.Data;
            if (data != null && data.Contains(ErrorHelper.StatusCodeKey))
            {
                try
                {
                    statusCode = (int)data[ErrorHelper.StatusCodeKey];
                }
                catch (Exception)
                {

                }
            }

            logger.LogError(context.Exception, context.Exception.Message);

            context.Result = new JsonResult(new ErrorViewModel
            {
                Message = context.Exception.Message
            });
            context.HttpContext.Response.StatusCode = statusCode;

            context.ExceptionHandled = true;
        }
    }
}
