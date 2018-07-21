using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.WebSocket;

namespace BAChatService
{
    class Logger
    {
        public static void Log(string message, WebSocketSession session = null)
        {
            if (session != null)
            {
                string identity = session.RemoteEndPoint.ToString();
                if (Login.IsLoggedIn(session))
                {
                    identity = identity + " (" + BASession.Sessions[session].username + ")";
                }
                Output("[" + DateTime.Now.ToLocalTime().ToString() + "]<" + identity + "> " + message);
            }
            else
            {
                Output(message);
            }
        }
        public static void Error(string message, WebSocketSession session = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log(message, session);
            Console.ResetColor();
        }
        private static void Output(string output)
        {
            Console.WriteLine(output);
        }
    }
}
