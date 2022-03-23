using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SimpleBus;

namespace WebApplication.Services
{
    public class MessageHandlerMiddleware
    {
        private static readonly IEnumerable<Type> _messageTypes;
        static MessageHandlerMiddleware()
        {
            var messageType = typeof(ICommandMessage);
                
            _messageTypes = typeof(MessageHandlerMiddleware).Assembly
                   .GetTypes()
                   .Where(t => messageType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                   .ToList();
        }

        private readonly RequestDelegate _next;
        private readonly IServiceBus _serviceBus;
        public MessageHandlerMiddleware(RequestDelegate next, IServiceBus serviceBus)
        {
            _next = next;
            _serviceBus = serviceBus;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var req = httpContext.Request;

            if (req.Path.StartsWithSegments("/api/Command", StringComparison.OrdinalIgnoreCase, out PathString remaining))
            {
                var modelType = _messageTypes
                    .Where(t => t.FullName.EndsWith(string.Format("{0}{1}", remaining.ToString().Replace("/", ""), "CommandModel")))
                    .FirstOrDefault();
                if (modelType != null)
                {
                    var reader = new StreamReader(req.Body);
                    var jsonString = reader.ReadToEnd();
                    var jsonObj = JsonSerializer.Deserialize(jsonString, modelType, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _serviceBus.Send((ICommandMessage)jsonObj);

                    httpContext.Response.StatusCode = 202;
                    return;
                }
            }

            await _next(httpContext); // calling next middleware
        }
    }
}
