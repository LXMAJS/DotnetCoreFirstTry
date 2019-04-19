using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Chat
{
    public class ChatWebSocketMiddleware
    {
        private static ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _sockets = new ConcurrentDictionary<string, System.Net.WebSockets.WebSocket>();

        private readonly RequestDelegate _next;

        public ChatWebSocketMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // 检测是否是websocket，若是，则进行当前上下文唤醒
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }
            // System.Net.WebSockets.WebSocket dummy;

            CancellationToken ct = context.RequestAborted;
            var currentSocket = await context.WebSockets.AcceptWebSocketAsync();

            // 接收前端传来的socketId
            string socketId = context.Request.Query["sid"].ToString();
            // 或者使用直接可获取到的IP地址，作为当前用户的socketid
            socketId = context.Request.Host.ToString();

            if (!_sockets.ContainsKey(socketId))
            {
                // 添加到连接池中
                _sockets.TryAdd(socketId, currentSocket);
            }

            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                string response = await ReceiveStringAsync(currentSocket, ct);
                MessageTemplateProtocal msg = JsonConvert.DeserializeObject<MessageTemplateProtocal>(response);

                if (string.IsNullOrEmpty(response))
                {
                    if (currentSocket.State != WebSocketState.Open)
                        break;

                    continue;
                }

                foreach (var socket in _sockets)
                {
                    if (socket.Value.State != WebSocketState.Open)
                    {
                        // 如果当前用户的状态不是连接状态，那么就不管了，直接考虑下一个用户
                        continue;
                    }
                    if (socket.Key == msg.ReceiverId || socket.Key == MessageTemplateProtocal.PublicReceiverId || socket.Key == socketId)
                    {
                        // 判断，如果key是接收者或自己，或者是发送给全部用户，那么就推送
                        await SendStringAsync(socket.Value, JsonConvert.SerializeObject(msg), ct);
                    }
                }
            }

            await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            currentSocket.Dispose();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[8192]); // 这里为什么是 8192，值得商榷
            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    ct.ThrowIfCancellationRequested();

                    result = await socket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if(result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }
                using(var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }

            throw new NotImplementedException();
        }
    }
}
