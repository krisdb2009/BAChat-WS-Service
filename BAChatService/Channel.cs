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
        public static void Route(WebSocketSession session)
        {
            string channelName = "";
            if(Protocol.Receive.Join(session, out channelName))
            {
                BAChannel channel = GetChannel(channelName);
                if (channel != null)
                {
                    channel.Join(session);
                }
                else
                {
                    new BAChannel(channelName).Join(session);
                }
                Logger.Log("Joined channel " + channelName + ".", session);
                return;
            }
            Protocol.Send.Join(session);
        }
        public static List<BAChannel> Channels = new List<BAChannel>();
        public static bool IsInChannel(WebSocketSession session)
        {
            if (BASession.Sessions[session].Channel != null)
            {
                return true;
            }
            return false;
        }
        public static BAChannel GetChannel(string channelName)
        {
            if (channelName != "")
            {
                foreach (BAChannel channel in Channels)
                {
                    if (channel.Name == channelName)
                    {
                        return channel;
                    }
                }
            }
            return null;
        }
        public List<WebSocketSession> Users = new List<WebSocketSession>();
        public BAChannel(string channelName)
        {
            Name = channelName;
            Channels.Add(this);
        }
        public void Join(WebSocketSession session)
        {
            BASession baSession = BASession.Sessions[session];
            Users.Add(session);
            baSession.Channel = this;
            Protocol.Send.Init(session);
            Chat.Broadcast(baSession.UserName + " has joined the channel.", new BAChannel[] { this });
        }
        public void Leave(WebSocketSession session)
        {
            BASession baSession = BASession.Sessions[session];
            Users.Remove(session);
            baSession.Channel = null;
            Protocol.Send.Join(session);
            Chat.Broadcast(baSession.UserName + " has left the channel.", new BAChannel[] { this });
        }
        public string Name;
    }
}
