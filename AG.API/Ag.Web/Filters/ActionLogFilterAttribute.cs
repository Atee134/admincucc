using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace Ag.Web.Filters
{
    public class ActionLogFilterAttribute : Attribute, IActionFilter
    {
        private readonly ILogger<ActionLogFilterAttribute> logger;
        private readonly string contextId = Guid.NewGuid().ToString().Substring(0, 8);

        public ActionLogFilterAttribute(ILogger<ActionLogFilterAttribute> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Action starts: {0} (request id: {1})", context.ActionDescriptor.DisplayName, contextId);
            logger.LogInformation("Request: [{0}] {1}://{2}{3}{4}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.IsHttps ? "https" : "http",
                context.HttpContext.Request.Host.HasValue ? context.HttpContext.Request.Host.Value : "(no host data)",
                context.HttpContext.Request.Path.HasValue ? context.HttpContext.Request.Path.Value : "",
                context.HttpContext.Request.QueryString
                );
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Action ends: {0} (request id: {1})", context.ActionDescriptor.DisplayName, contextId);
        }
    }
}
