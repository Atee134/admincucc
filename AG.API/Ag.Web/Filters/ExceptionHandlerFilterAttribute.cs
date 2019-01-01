using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ag.Web.Filters
{
    internal class ExceptionHandlerFilterAttribute : ExceptionFilterAttribute
    {
        //private ILogger<ExceptionHandlerFilterAttribute> logger;

        //public ExceptionHandlerFilterAttribute(ILogger<ExceptionHandlerFilterAttribute> logger)
        //{
        //    this.logger = logger;
        //}

        //public override void OnException(ExceptionContext context)
        //{
        //    var controllerName = context.RouteData.Values["controller"];
        //    var actionName = context.RouteData.Values["action"];

        //    logger.LogError(context.Exception, $"An unexpected exception occured while processing the request in {controllerName}/{actionName}. Message: {context.Exception.Message}");
        //}
    }
}
