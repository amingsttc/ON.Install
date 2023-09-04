using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ON.Mercury.Service.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ON.Mercury.Service.Middleware
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        // TODO: Cleanup and Refactor catch blocks
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }  
            catch (ChannelNotFoundException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Type = "CHANNEL",
                    Title = "Channel Not Found",
                    Detail = e.Message
                };
                var json = JsonConvert.SerializeObject(problemDetails);
                await context.Response.WriteAsync(json);
            }
            catch (MessageNotFoundException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Type = "Message",
                    Title = "Message Not Found",
                    Detail = e.Message
                };
                var json = JsonConvert.SerializeObject(problemDetails);
                await context.Response.WriteAsync(json);
            }
            catch (MessageNotPinnedException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Type = "Message",
                    Title = "Message Not Pinned",
                    Detail = e.Message
                };
                var json = JsonConvert.SerializeObject(problemDetails);
                await context.Response.WriteAsync(json);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails problemDetails = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "SERVER_ERROR",
                    Title = "Server Error",
                    Detail = "An Internal Server Error Has Occured"
                };

                var json = JsonConvert.SerializeObject(problemDetails);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
