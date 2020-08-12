using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace WebSocketServer.SocketsManager
{
    public static class SocketsExtention
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<ConnectionManager>();
            foreach (Type type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(SocketHandler))
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }

        public static IApplicationBuilder MapSockets(this IApplicationBuilder app, PathString path,
            SocketHandler socket)
        {
            return app.Map(path, x => x.UseMiddleware<SocketMiddleware>(socket));
        }
    }
}
