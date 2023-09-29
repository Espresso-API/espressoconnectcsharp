EspressoApi-dotnet
Espresso API Using C# Asp. Net
Espresso API is a set of REST-like APIs that expose many capabilities required to build a complete investment and trading platform. Execute orders in real time, stream live market data (WebSockets), and more, with the simple HTTP API collection.

Prerequisite
Download Espresso.dll file and add reference to your desktop/web application

[Click here](Espresso\Espresso\bin\Debug) to get all necessory dll that you may need, or you can install from package manager console as follow

PM> Install-Package Newtonsoft.Json. For More details (https://www.nuget.org/packages/Newtonsoft.Json/)

API Usage
Initialize Espresso API using api_key , access_token and vendor_key(optional).

            string api_key = "enter-your-api-key";
            string access_token = "enter-your-accesstoken";
            string vendor_key = null;
            EspressoApi connect = new EspressoApi(access_token, api_key, vendor_key);

LoginUrl:this will provide you the login url which can be used to login the espresso account. In-case oxf custormer login leave vendorkey as null .

            string apiKey = "enter-your-api-key";
            string vendorKey = null;
            long state = 12345;
            var response = connect.GetLoginUrl(apiKey, vendorKey, state);
            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));



GenerateToken: provide the requestToken received after successful login with apiKey, state, secret key and vendorKey if it is a vendor login,in case of customer login leave it null.��� This will provide the accessToken after the decrypt and encrypt part.  you can decrypt/encrypt the requestToken using Base64.getDecoder() n Base64.getEncoder().

            string apiKey = "enter-your-apikey";
            string requestToken = "enter-the-requesttoken";
            int state = 12345;
            string secretKey = "enter-your-secretkey";
            string vendorKey = null;
            var response = connect.GenerateAccessTokenWithoutVersionId(apiKey, requestToken, state, secretKey, vendorKey);
            var responseObject = JsonConvert.DeserializeObject(response);
            var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
            Console.WriteLine(responseString);

//add apikey, accesstoken, vendorkey(optional) in constructor in program.cs.

PlaceOrder
             
            var order = new OrderInfo
            {
                customerId = 1111111,
                scripCode = 252905,
                tradingSymbol = "GOLDPETAL",
                exchange = "MX",             (NC,BC,NF,RN,MX)
                transactionType = "B",       (B, S, BM, SM, SAM)
                quantity = 1,
                disclosedQty = 0,
                price = "5990",
                triggerPrice = "0",
                rmsCode = "ANY",
                afterHour = "N",
                orderType = "NORMAL",
                channelUser = "1111111",         ��//enter the customerid
                validity = "GFD",                GFD,myGTD,IOC
                requestType = "NEW",
                productType = "INVESTMENT",    For Equity Exchange - CNC (Normal), For F&O - CNF(normal),MIS,MIS+
                // Below parameters need to be added for FNO trading or else keep them null
                instrumentType = "FS",        (Future Stocks(FS)/ Future Index(FI)/ Option Index(OI)/ Option Stocks(OS)/ Future Currency(FUTCUR)/ Option Currency(OPTCUR))�
                strikePrice = "-1",
                optionType = "XX",     ���(XX/PE/CE)�
                expiry = "31/05/2023"
            };

            var response = connect.placeOrder(order);
            var responseObject = JsonConvert.DeserializeObject(response);
            var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
            Console.WriteLine(responseString);

ModifyOrder
            
           var order = new OrderInfo
            {
                orderId = 111111,
                customerId = 1111111,
                scripCode = 252905,
                tradingSymbol = "GOLDPETAL",
                exchange = "MX",                (NC,BC,NF,RN,MX)
                transactionType = "B",       (B, S, BM, SM, SAM)
                quantity = 1,
                disclosedQty = 0,
                 executedQty=0,
                price = "5990",
                triggerPrice = "0",
                rmsCode = "ANY",
                afterHour = "N",
                orderType = "NORMAL",
                channelUser = "1111111",         ��//enter the customerid
                validity = "GFD",           GFD,myGTD,IOC
                requestType = "Modify",
                productType = "INVESTMENT",     For Equity Exchange - CNC (Normal), For F&O - CNF(normal),MIS,MIS+
                  // Below parameters need to be added for FNO trading or else keep them null
                instrumentType = "FS",        (Future Stocks(FS)/ Future Index(FI)/ Option Index(OI)/ Option Stocks(OS)/ Future Currency(FUTCUR)/ Option Currency(OPTCUR))�
                strikePrice = "-1",
                optionType = "XX",     ���(XX/PE/CE)�
                expiry = "31/05/2023"
            };


                var response = connect.modifyOrder(order);
                var responseObject = JsonConvert.DeserializeObject(response);
                var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                Console.WriteLine(responseString);

Cancel Order
                
               var order = new OrderInfo
            {
                orderId = 111111,
                customerId = 1111111,
                scripCode = 252905,
                tradingSymbol = "GOLDPETAL",
                exchange = "MX",              (NC,BC,NF,RN,MX)
                transactionType = "B",       (B, S, BM, SM, SAM)
                quantity = 1,
                disclosedQty = 0,
                executedQty=0,
                price = "5990",
                triggerPrice = "0",
                rmsCode = "ANY",
                afterHour = "N",
                orderType = "NORMAL",
                channelUser = "1111111",         ��//enter the customerid
                validity = "GFD",              GFD,myGTD,IOC
                requestType = "CANCEL",
                productType = "INVESTMENT",     For Equity Exchange - CNC (Normal), For F&O - CNF(normal),MIS,MIS+
                  // Below parameters need to be added for FNO trading or else keep them null
                instrumentType = "FS",        (Future Stocks(FS)/ Future Index(FI)/ Option Index(OI)/ Option Stocks(OS)/ Future Currency(FUTCUR)/ Option Currency(OPTCUR))�
                strikePrice = "-1",
                optionType = "XX",     ���(XX/PE/CE)�
                expiry = "31/05/2023"
            };


                var response = connect.cancelOrder(order);
                var responseObject = JsonConvert.DeserializeObject(response);
                var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                Console.WriteLine(responseString);

CancelByOrderId

                int orderId = 1111111;
                var response = connect.cancelByOrderId(orderId);
                var responseObject = JsonConvert.DeserializeObject(response);
                var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                Console.WriteLine(responseString);

Funds

                string exchange = "RN";
                int customerId = 1111111;
                var response = connect.funds(exchange, customerId);
                var responseObject = JsonConvert.DeserializeObject(response);
                var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                Console.WriteLine(responseString);

Order

                int customerId = 1111111;
                var response = connect.orders(customerId);
                var responseObject = JsonConvert.DeserializeObject(response);
                var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                Console.WriteLine(responseString);

Positions

                int customerId = 1111111;
                var response = connect.positions(customerId);
                var responseObject = JsonConvert.DeserializeObject(response);
                var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                Console.WriteLine(responseString);

History

                string exchange = "MX";
                int customerId = 1487617;
                int orderId = 268487173;
                var response = connect.history(exchange, customerId, orderId);
                var responseObject = JsonConvert.DeserializeObject(response);
                var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                Console.WriteLine(responseString);

Trade

                string exchange = "RN";
                int customerId = 1111111;
                int orderId = 1111111;
                var response = connect.trade(exchange, customerId, orderId);
                var responseObject = JsonConvert.DeserializeObject(response);
                var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                Console.WriteLine(responseString);

Holdings

                int customerId = 1111111;
                var response = connect.holdings(customerId);
                var responseObject = JsonConvert.DeserializeObject(response);
                var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                Console.WriteLine(responseString);

ActiveScrips

                string exchange = "RN";
                var response = connect.activeScrips(exchange);
                var responseObject = JsonConvert.DeserializeObject(response);
                var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                Console.WriteLine(responseString);

Symbol
    
                 string exchange = "RN";
                 var response = connect.symbol(exchange);
                 var responseObject = JsonConvert.DeserializeObject(response);
                 var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                 Console.WriteLine(responseString);

HistoricalData

                string exchange = "RN";
                int scripCode = 123123;
                string interval = "daily";
                var response = connect.historicalData(exchange, scripCode, interval);
                var responseObject = JsonConvert.DeserializeObject(response);
                var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
                Console.WriteLine(responseString);

WEbSocket Live Streaming Data

//Websocket will be tested in WebTest.cs


            string accesstoken = "enter-your-accesstoken";
               string apiKey = "enter-your-apikey";

Connection Request
                
                 _WS.ConnectforOrderQuote(accesstoken,apiKey);

Subscribe Request

                string script = "{\"action\":\"subscribe\",\"key\":[\"feed\"],\"value\":[\"\"]}";
                _WS.RunScript(script);

Feed Request: You can send the request as feed/ack as per as requirement.

                string feedData = "{\"action\":\"feed\",\"key\":[\"ltp\"],\"value\":[\"MX255320\"]}";
                _WS.RunScript(feedData);

Unsubscribe Request

                string unsubscribe = "{\"action\":\"unsubscribe\",\"key\":[\"ltp\"],\"value\":[\"RN1064\"]}";
                _WS.RunScript(unsubscribe);

Disconnect Request

                Console.WriteLine("Press any key to stop and close the socket connection.");
                Console.ReadKey();

            