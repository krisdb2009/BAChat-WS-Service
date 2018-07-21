using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.WebSocket;
using Newtonsoft.Json;

namespace BAChatService
{
    namespace Protocol
    {
        class Send
        {
            public static void Login(WebSocketSession session, string token = null)
            {
                if (token != null && token.Length == 32)
                {
                    session.Send("{\"command\":\"login\",\"token\":\"" + token + "\"}");
                }
                else
                {
                    session.Send("{\"command\":\"login\"}");
                }
            }
        }
        class Receive
        {
            public static bool Login(WebSocketSession session, out bool isToken, out Dictionary<string, string> credentials)
            {
                isToken = false;
                credentials = null;
                string message = BASession.Sessions[session].lastMessage;
                try
                {
                    credentials = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                    if (credentials.ContainsKey("username") && credentials.ContainsKey("password"))
                    {
                        return true;
                    }
                    else if (credentials.ContainsKey("token"))
                    {
                        isToken = true;
                        return true;
                    }
                }
                catch(JsonReaderException e)
                {
                    Logger.Error("Protocol Error: " + e.Message, session);
                    return false;
                }
                return false;
            }
            public static bool Join(WebSocketSession session, out string channelName)
            {
                channelName = "";
                Dictionary<string, string> command;
                string message = BASession.Sessions[session].lastMessage;
                try
                {
                    command = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                }
                catch (JsonReaderException e)
                {
                    Logger.Error("Protocol Error: " + e.Message, session);
                    return false;
                }
                if(command.ContainsKey("command") && command["command"] == "join" && command.ContainsKey("channel") && command["channel"].Length != 0)
                {
                    channelName = command["channel"];
                    return true;
                }
                return false;
            }
        }
    }
}
