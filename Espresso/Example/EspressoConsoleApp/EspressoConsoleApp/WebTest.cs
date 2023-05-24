//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Espresso;
//using Espresso.Espresso.Espresso;
//using Newtonsoft.Json;

//namespace Espresso.Example.EspressoConsoleApp.EspressoConsoleApp
//{
//    class WebTest
//    {

//        static void Main(string[] args)
//        {

//            string accesstoken = "eyJ0eXAiOiJzZWMiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiJaT3hSR3R1WlQ1a2ZrTmlzUXVpbU15cUJzdFJFcTJlMDc4K0x1T2pGaXdIbEk5MEkzdTB4cXNYcXB4QXdMcnlKaUxoSy9wcGJtOG5VTHVhcFN5VlBQTFptOVBHSUxEMk81VDI0MlR6a21GbjcxRE1leE1yaHJwVXlLVFVZUGE4ajczelVpbVptb0ZjUFJqYTh2QVlLa3p0bUxmajA0TWkzOTRkSDZPb0dBS3M9IiwiaWF0IjoxNjg0MjA5Nzg0LCJleHAiOjE2ODQyNjE3OTl9.uQSTXHpVa9NoaAO5X6_admhJ7y0rZ0QOhjZPGyMSZew";
//            string apikey = "Da8WmWZsE9VjVZJPH0cK1oVMZotE70O7";


//            WEBSocket _WS = new WEBSocket();
//            var exitEvent = new ManualResetEvent(false);

//            _WS.ConnectforOrderQuote(accesstoken, apikey);
//            if (_WS.IsConnected())
//            {
//                string script = "{\"action\":\"subscribe\",\"key\":[\"feed\"],\"value\":[\"\"]}";
//                _WS.RunScript(script);

//                string feedData = "{\"action\":\"feed\",\"key\":[\"ltp\"],\"value\":[\"NC22,NF37833,NF37834,MX253461,RN7719\"]}";
//                _WS.RunScript(feedData);

//                //string unsubscribe = "{\"action\":\"unsubscribe\",\"key\":[\"ltp\"],\"value\":[\"RN1064\"]}";
//                //_WS.RunScript(unsubscribe);

//                _WS.MessageReceived += WriteResult;

//                Console.WriteLine("Press any key to stop and close the socket connection.");
//                Console.ReadKey();

//                _WS.Close(true);// to stop and close socket connection
//            }
//            //exitEvent.WaitOne();
//        }
//        static void WriteResult(object sender, MessageEventArgs e)
//        {
//            Console.WriteLine("Tick Received : " + e.Message);

//        }


//    }
//}
