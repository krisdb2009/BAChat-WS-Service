using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.WebSocket;

namespace BAChatService
{
    class Chat
    {
        public static void Route(WebSocketSession session)
        {
            string message = "";
            if(Protocol.Receive.Chat(session, out message))
            {
                Send(session, message);
            }
        }
        public static void Send(WebSocketSession session, string message, WebSocketSession[] sessions = null)
        {
            BASession baSession = BASession.Sessions[session];
            foreach (WebSocketSession toSession in BASession.Sessions[session].Channel.Users)
            {
                Protocol.Send.Chat(toSession, baSession.UserName, message);
            }
        }
        public static void Broadcast(string message, BAChannel[] baChannels = null)
        {
            if(baChannels == null)
            {
                baChannels = BAChannel.Channels.ToArray();
            }
            foreach(BAChannel baChannel in baChannels)
            {
                foreach(WebSocketSession session in baChannel.Users)
                {
                    Protocol.Send.Chat(session, "Server", message);
                }
            }
        }
    }
}
