using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.WebSocket;

namespace BAChatService
{
    class BASession
    {
        public static Dictionary<WebSocketSession, BASession> Sessions = new Dictionary<WebSocketSession, BASession>();
        public BASession(WebSocketSession session)
        {
            Sessions.Add(session, this);
        }
        public static void DestroyBASession(WebSocketSession session)
        {
            Sessions.Remove(session);
        }
        public string username = "";
        public string lastMessage = "";
    }
}
