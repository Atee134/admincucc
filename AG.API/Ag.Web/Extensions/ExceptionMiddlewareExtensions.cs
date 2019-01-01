using Ag.BusinessLogic.Exceptions;
using Ag.Common.Dtos;
using Ag.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ag.Web.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        List<string> errorMessages = new List<string>();
                        var aggregateException = contextFeature.Error as AggregateException;

                        if (aggregateException != null)
                        {
                            foreach (Exception e in aggregateException.InnerExceptions)
                            {
                                errorMessages.Add(e.Message);
                            }
                        }
                        else if (contextFeature.Error is AgUnfulfillableActionException)
                        {
                            errorMessages.Add($"Unable to perform action, {contextFeature.Error.Message}");
                            logger.LogWarning($"Unable to perform action. Sending bad request to client, with message: {contextFeature.Error.Message}");
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        }
                        else
                        {
                            errorMessages.Add(contextFeature.Error.Message);
                        }

                        context.Response.Headers.Add("Application-Error", String.Join(';', errorMessages));
                        context.Response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    }
                });
            });
        }
    }
}
