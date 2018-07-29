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
            public static void Join(WebSocketSession session)
            {
                session.Send("{\"command\":\"join\"}");
            }
            public static void Init(WebSocketSession session)
            {
                BASession baSession = BASession.Sessions[session];
                BAChannel baChannel = baSession.Channel;
                Dictionary<string, string> json = new Dictionary<string, string>();
                json.Add("command", "init");
                json.Add("username", baSession.UserName);
                if (baChannel != null)
                {
                    json.Add("channel", baChannel.Name);
                }
                session.Send(JsonConvert.SerializeObject(json));
            }
            public static void Chat(WebSocketSession session, string username, string message, string from = null)
            {
                if(username != "" && message != "")
                {
                    Dictionary<string, string> json = new Dictionary<string, string>();
                    json.Add("command", "chat");
                    json.Add("username", username);
                    json.Add("message", message);
                    session.Send(JsonConvert.SerializeObject(json));
                }
            }
        }
        class Receive
        {
            public static bool Login(WebSocketSession session, out bool isToken, out Dictionary<string, string> credentials)
            {
                isToken = false;
                credentials = null;
                string message = BASession.Sessions[session].LastMessage;
                try
                {
                    credentials = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                    if (credentials != null && credentials.ContainsKey("command") && credentials["command"] == "login") {
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
                string lastMessage = BASession.Sessions[session].LastMessage;
                try
                {
                    command = JsonConvert.DeserializeObject<Dictionary<string, string>>(lastMessage);
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
            public static bool Chat(WebSocketSession session, out string message)
            {
                message = "";
                string lastMessage = BASession.Sessions[session].LastMessage;
                Dictionary<string, string> command;
                try
                {
                    command = JsonConvert.DeserializeObject<Dictionary<string, string>>(lastMessage);
                }
                catch (JsonReaderException e)
                {
                    Logger.Error("Protocol Error: " + e.Message, session);
                    return false;
                }
                if (command.ContainsKey("command") && command["command"] == "chat" && command.ContainsKey("message") && command["message"].Length != 0)
                {
                    message = command["message"];
                    return true;
                }
                return false;
            }
        }
    }
}
