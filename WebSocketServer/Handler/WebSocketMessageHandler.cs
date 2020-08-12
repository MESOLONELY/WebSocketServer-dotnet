using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WebSocketServer.SocketsManager;

namespace WebSocketServer.Handler
{
    public class WebSocketMessageHandler : SocketHandler
    {
        public WebSocketMessageHandler(ConnectionManager connections) : base(connections)
        {
        }

        public override Task OnConnected(WebSocket socket)
        {
            return base.OnConnected(socket);
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            string socketId = Connections.GetId(socket);
            await SendMessageExceptId(Encoding.UTF8.GetString(buffer,0,result.Count), socketId);
        }
    }
}
