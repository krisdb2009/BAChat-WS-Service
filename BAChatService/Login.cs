using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.WebSocket;
using System.Net;
namespace BAChatService
{
    class Login
    {
        public static void LoginRoute(WebSocketSession session)
        {
            BASession baSession = BASession.Sessions[session];
            if (baSession.messages.Count == 1)
            {
                if(baSession.messages[0].Length == 32)
                {
                    throw new NotImplementedException("Auth Token Handleing not ready...");
                }
            }
            else if(baSession.messages.Count == 2)
            {
                PerformLogin(baSession.messages[0], baSession.messages[1]);
            }
            else
            {
                baSession.messages.Clear();
            }
        }
        public static bool IsLoggedIn(WebSocketSession session)
        {
            if (BASession.Sessions[session].username != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool PerformLogin(string username, string password)
        {
            try
            {
                WebClient wc = new WebClient();
                Uri uri;
                Uri.TryCreate("https://api.belowaverage.org/v1/AUTH/", UriKind.Absolute, out uri);
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.UploadString(uri, "username=" + username + "&password=" + password);
            }
            catch(WebException e)
            {
                Console.WriteLine(e.Message);
            }
            return true;
        }
    }
}