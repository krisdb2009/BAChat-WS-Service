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
            Console.WriteLine("Starting server on port " + port + "...");
            server.Setup(port);
            server.Start();
            server.NewSessionConnected += Ws_NewSessionConnected;
            server.NewMessageReceived += Ws_NewMessageReceived;
            Console.WriteLine("Ready.");

            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void Ws_NewMessageReceived(WebSocketSession session, string message)
        {
            BASession.Sessions[session].messages.Add(message);
            if (!Login.IsLoggedIn(session))
            {
                Login.LoginRoute(session);
            }
        }

        private static void Ws_NewSessionConnected(WebSocketSession session)
        {
            new BASession(session);
            session.Send("login");
        }
    }
}
