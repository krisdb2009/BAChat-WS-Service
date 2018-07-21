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
        public static List<BAChannel> channels;
        public static BAChannel GetChannel(WebSocketSession session)
        {
            foreach(BAChannel baChannel in channels)
            {
                if(baChannel.users.Contains(session))
                {
                    return baChannel;
                }
            }
            return null;
        }
        public BAChannel(string channelName)
        {
            name = channelName;
        }
        public List<WebSocketSession> users;
        public string name;
    }
}
