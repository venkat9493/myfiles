using Microsoft.AspNetCore.Http;
using Nanostring.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlCenterMockApi.Middleware
{
    public class LoggingMiddleware
    { /// <summary>
      /// ILogger
      /// </summary>
        private readonly ILogger<LoggingMiddleware> _logger;
        /// <summary>
        /// RequestDelegate
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// LoggingMiddleware
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            var operationId = default(string);
            this._logger.WriteInfo(Constants.MiddlewareStarted);
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("Correlation-ID", operationId);
                return Task.CompletedTask;
            });
            await this._next.Invoke(context).ConfigureAwait(false);
            this._logger.WriteInfo(Constants.MiddlewareEnded);
        }
    }
}
