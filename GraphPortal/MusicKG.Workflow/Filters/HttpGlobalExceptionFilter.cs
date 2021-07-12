using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MusicKG.Workflow.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MusicKG.Workflow.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger logger;

        public HttpGlobalExceptionFilter(IHostingEnvironment hostingEnvironment, ILogger<HttpGlobalExceptionFilter> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var data = context.Exception.Data;
            if (data != null && data.Contains("statusCode"))
            {
                try
                {
                    statusCode = (int)data["statusCode"];
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
