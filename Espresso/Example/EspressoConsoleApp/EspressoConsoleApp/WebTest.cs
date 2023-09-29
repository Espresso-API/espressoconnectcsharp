using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Espresso;
using Espresso.Espresso.Espresso;
using Newtonsoft.Json;

namespace Espresso.Example.EspressoConsoleApp.EspressoConsoleApp
{
    class WebTest
    {

        static void Main(string[] args)
        {

            string accesstoken = "enter-your-accessToken";
            string apikey = "enter-your-apiKey";


            WEBSocket _WS = new WEBSocket();
            var exitEvent = new ManualResetEvent(false);

            _WS.ConnectforOrderQuote(accesstoken, apikey);
            if (_WS.IsConnected())
            {
                string script = "{\"action\":\"subscribe\",\"key\":[\"ack\"],\"value\":[\"\"]}";
                _WS.RunScript(script);

                string feedData = "{\"action\":\"ack\",\"key\":[\"\"],\"value\":[\"1111111\"]}";
                _WS.RunScript(feedData);

                //string unsubscribe = "{\"action\":\"unsubscribe\",\"key\":[\"ltp\"],\"value\":[\"RN1064\"]}";
                //_WS.RunScript(unsubscribe);

                _WS.MessageReceived += WriteResult;

                Console.WriteLine("Press any key to stop and close the socket connection.");
                Console.ReadKey();

                _WS.Close(true);// to stop and close socket connection
            }
            //exitEvent.WaitOne();
        }
        static void WriteResult(object sender, MessageEventArgs e)
        {
            Console.WriteLine("Tick Received : " + e.Message);

        }


    }
}
