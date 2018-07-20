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
            Dictionary<string, string> credentials;
            bool isToken;
            if (Protocol.Receive.Login(session, out isToken, out credentials))
            {
                if(isToken)
                {
                    string result = PerformLogin(credentials["token"]);
                    if (result != "")
                    {
                        Console.WriteLine("Welcome " + result);
                        baSession.username = result;
                    }
                    else
                    {
                        Protocol.Send.Login(session);
                    }
                }
                else
                {
                    string result = PerformLogin(credentials["username"], credentials["password"]);
                    if(result.Length == 32)
                    {
                        Console.WriteLine("Sending token and login command");
                        Protocol.Send.Login(session, result);
                    }
                    else
                    {
                        Protocol.Send.Login(session);
                    }
                }
            }
            else
            {
                Protocol.Send.Login(session);
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
        public static string PerformLogin(string username_or_token, string password = null)
        {
            try
            {
                WebClient wc = new WebClient();
                Uri uri;
                Uri.TryCreate(Service.appSettings["BackEndAuthURL"], UriKind.Absolute, out uri);
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                if (username_or_token.Length == 32)
                {
                    return wc.UploadString(uri, username_or_token);
                }
                else
                {
                    return wc.UploadString(uri, "username=" + username_or_token + "&password=" + password);
                }
            }
            catch(WebException e)
            {
                Console.WriteLine("Back-End-Auth: " + e.Message);
            }
            return "";
        }
    }
}