using System;
using System.Collections.Specialized;
using System.Configuration;
using SuperSocket.WebSocket;

namespace BAChatService
{
    class Service
    {
        public static WebSocketServer server = new WebSocketServer();
        public static NameValueCollection appSettings = ConfigurationManager.AppSettings;
        static void Main(string[] args)
        {
            int port = Convert.ToInt32(appSettings["ListeningPort"]);
            Logger.Log("Starting on port " + port + "...");
            server.Setup(port);
            server.Start();
            server.NewSessionConnected += Ws_NewSessionConnected;
            server.NewMessageReceived += Ws_NewMessageReceived;
            server.SessionClosed += Server_SessionClosed;
            Logger.Log("Ready for connections.");
            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void Ws_NewMessageReceived(WebSocketSession session, string message)
        {
            BASession baSession = BASession.Sessions[session];
            baSession.LastMessage = message;
            if (!Login.IsLoggedIn(session))
            {
                Login.Route(session);
            }
            if(!BAChannel.IsInChannel(session))
            {
                BAChannel.Route();
            }
        }

        private static void Ws_NewSessionConnected(WebSocketSession session)
        {
            new BASession(session);
            Protocol.Send.Login(session);
            Logger.Log("Connection established. Login command sent.", session);
        }

        private static void Server_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            Logger.Log("Connection lost: " + value.ToString(), session);
            //Leave the rooms
            //Remove the session

        }
    }
}
