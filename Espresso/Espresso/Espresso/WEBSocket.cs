using Espresso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Net;
using Websocket.Client;
using System.IO;


namespace Espresso.Espresso.Espresso
{
    class WEBSocket : IWebSocket
    {
        ManualResetEvent receivedEvent = new ManualResetEvent(false);
        int receivedCount = 0;
        WebsocketClient _ws;



        public event EventHandler<MessageEventArgs> MessageReceived;

        public WEBSocket()
        {

        }
        public bool IsConnected()
        {
            if (_ws is null)
                return false;

            return _ws.IsStarted;
        }

        public void ConnectforOrderQuote(string accessToken, string apiKey)
        {
            try
            {
                var receivedEvent = new ManualResetEvent(false);

                string finalurl = $"wss://streams.myespresso.com/espstream/api/stream?ACCESS_TOKEN={accessToken}&API_KEY={apiKey}";
                var url = new Uri(finalurl);
                Console.WriteLine(url);

                _ws = new WebsocketClient(url);

                _ws.MessageReceived.Subscribe((ResponseMessage msg) => Receive(msg.Text));

                _ws.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RunScript(string script)
        {
            string strwatchlistscrips = "";

            if (_ws.IsStarted)
            {
                try
                {

                    strwatchlistscrips = script;
                    string scriptReq = strwatchlistscrips;
                    //Console.WriteLine(scriptReq);
                    _ws.Send(scriptReq);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public void Receive(string Message)
        {
            MessageEventArgs args = new MessageEventArgs();

            //args.Message = Helpers.DecodeBase64(Message);
            args.Message = Message;
            EventHandler<MessageEventArgs> handler = MessageReceived;
            if (handler != null)
            {
                handler(this, args);
            }
            receivedCount++;
            if (receivedCount >= 10)
                receivedEvent.Set();
        }

        public void HeartBeat(string accesstoken)
        {
            string hbmsg = "heartbeat";
            if (_ws.IsStarted)
            {
                try
                {
                    _ws.Send(hbmsg);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Send(string Message)
        {
            if (_ws.IsStarted)
            {
                try
                {
                    _ws.Send(Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Close(bool Abort = false)
        {
            if (_ws.IsRunning)
            {
                if (Abort)
                    _ws.Stop(WebSocketCloseStatus.NormalClosure, "Close");
                else
                {
                    _ws.Dispose();
                }
            }
        }
    }
}
