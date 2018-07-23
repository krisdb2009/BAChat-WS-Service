using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.WebSocket;

namespace BAChatService
{
    class BAChannel
    {
        public static void Route()
        {

        }
        public static List<BAChannel> Channels = new List<BAChannel>();
        public static BAChannel GetChannel(WebSocketSession session)
        {
            foreach(BAChannel baChannel in Channels)
            {
                if(baChannel.Users.Contains(session))
                {
                    return baChannel;
                }
            }
            return null;
        }
        public static bool IsInChannel(WebSocketSession session)
        {
            if (GetChannel(session) != null)
            {
                return true;
            }
            return false;
        }
        public BAChannel(string channelName)
        {
            Name = channelName;
            Channels.Add(this);
        }
        public List<WebSocketSession> Users = new List<WebSocketSession>();
        public string Name;
    }
}
