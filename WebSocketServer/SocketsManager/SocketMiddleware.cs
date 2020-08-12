using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketServer.SocketsManager
{
    public class SocketMiddleware
    {
        private readonly RequestDelegate _next;

        public SocketMiddleware(RequestDelegate next, SocketHandler handler)
        {
            _next = next;
            Handler = handler;
        }

        private SocketHandler Handler { get; set; }

        public async Task InvokeAsync(HttpContext context)
        {
            if(!context.WebSockets.IsWebSocketRequest)
            {
                return;
            }
            WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();
            await Handler.OnConnected(socket);
            await Receive(socket, async (result, buffer) =>
            {
                if(result.MessageType == WebSocketMessageType.Text)
                {
                    await Handler.Receive(socket, result, buffer);
                }
                else if(result.MessageType == WebSocketMessageType.Close)
                {
                    await Handler.OnDisconnected(socket);
                }
            });
        }

        private async Task Receive(WebSocket webSocket, Action<WebSocketReceiveResult, byte[]> messageHandler)
        {
            byte[] buffer = new byte[1024 * 16];
            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                messageHandler(result, buffer);
            }
        }
    }
}
