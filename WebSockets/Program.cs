using System;
using SuperSocket.WebSocket;

namespace WebSockets
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            WebSocketServer ws = new WebSocketServer();
            ws.Setup(1234);
            ws.Start();
            ws.NewSessionConnected += Ws_NewSessionConnected;
            ws.NewMessageReceived += Ws_NewMessageReceived;
            Console.WriteLine("Ready.");

            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void Ws_NewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine(session.RemoteEndPoint.Address + ":" + session.RemoteEndPoint.Port + ": " + value);
            session.Send(value);
        }

        private static void Ws_NewSessionConnected(WebSocketSession session)
        {
            Console.WriteLine(session.RemoteEndPoint.Address + ":" + session.RemoteEndPoint.Port + " connected.");
        }
    }
}
