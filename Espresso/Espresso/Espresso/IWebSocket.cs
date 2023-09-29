using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Espresso.Espresso.Espresso
{
    interface IWebSocket
    {
        bool IsConnected();
        void ConnectforOrderQuote(string accesstoken, string apikey);

        void Send(string Message);
        void Close(bool Abort = false);
        void HeartBeat(string accesstoken);

        void RunScript(string script);
    }
}
