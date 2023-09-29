//using Espresso.Espresso.Espresso;
//using Microsoft.VisualBasic;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//namespace Espresso.Example.EspressoConsoleApp.EspressoConsoleApp
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {

//            string api_key = "enter-your-apiKey";
//            string access_token = "enter-your-accessToken";
//            string vendor_key = null;

//            EspressoApi connect = new EspressoApi(access_token, api_key, vendor_key);

//string apiKey = "enter-your-apiKey";
//string vendorKey = null;
//long state = 12345;
//var response = connect.GetLoginUrl(apiKey, vendorKey, state);
//Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));



//string apiKey = "enter-your-apiKey";
//string requestToken = "enter-your-requestToken";
//int state = 12345;
//string secretKey = "enter-your-secretKey";
//string vendorKey = null;
//var response = connect.GenerateAccessToken(apiKey, requestToken, state, secretKey, vendorKey);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);



//var order = new OrderInfo
//{

//    customerId = 1111111,
//    scripCode = 2475,
//    tradingSymbol = "ONGC",
//    exchange = "NC",
//    transactionType = "B",
//    quantity = 1,
//    disclosedQty = 0,
//    price = "150",
//    triggerPrice = "0",
//    rmsCode = "ANY",
//    afterHour = "N",
//    orderType = "NORMAL",
//    channelUser = "1111111",
//    validity = "GFD",
//    requestType = "NEW",
//    productType = "CNC",
//    instrumentType = null,
//    strikePrice = null,
//    optionType = null,
//    expiry = null
//};

//var response = connect.placeOrder(order);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);



//var order = new OrderInfo
//{
//    orderId = "1111111",
//    customerId = 1111111,
//    scripCode = 2475,
//    tradingSymbol = "ONGC",
//    exchange = "NC",
//    transactionType = "B",
//    quantity = 1,
//    disclosedQty = 0,
//    executedQty = 0,
//    price = "190",
//    triggerPrice = "0",
//    rmsCode = "ANY",
//    afterHour = "N",
//    orderType = "NORMAL",
//    channelUser = "1111111",
//    validity = "GFD",
//    requestType = "MODIFY",
//    productType = "CNC",
//    instrumentType = null,
//    strikePrice = null,
//    optionType = null,
//    expiry = null
//};

//var response = connect.modifyOrder(order);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);


//var order = new OrderInfo
//{
//    orderId = "1111111",
//    customerId = 1111111,
//    scripCode = 2475,
//    tradingSymbol = "ONGC",
//    exchange = "NC",
//    transactionType = "B",
//    quantity = 1,
//    disclosedQty = 0,
//    executedQty = 0,
//    price = "190",
//    triggerPrice = "0",
//    rmsCode = "ANY",
//    afterHour = "N",
//    orderType = "NORMAL",
//    channelUser = "1111111",
//    validity = "GFD",
//    requestType = "MODIFY",
//    productType = "CNC",
//    instrumentType = null,
//    strikePrice = null,
//    optionType = null,
//    expiry = null
//};

//var response = connect.cancelOrder(order);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);


//string exchange = "RN";
//int customerId = 1111111;
//var response = connect.funds(exchange,customerId);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);

//int orderId = 1111111;
//var response = connect.cancelByOrderId(orderId);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);

//int customerId = 1111111;
//var response = connect.orders(customerId);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);

//int customerId = 1111111;
//var response = connect.positions(customerId);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);

//string exchange = "RN";
//int customerId = 1111111;
//int orderId = 1111111;
//var response = connect.history(exchange,customerId,orderId);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);

//string exchange = "RN";
//int customerId = 1111111;
//int orderId = 1111111;
//var response = connect.trade(exchange,customerId,orderId);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);

//int customerId = 1111111;
//var response = connect.holdings(customerId);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);

//string exchange = "RN";
//var response = connect.activeScrips(exchange);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);


//string exchange = "RN";
//var response = connect.symbol(exchange);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);

//string exchange = "RN";
//int scripCode = 123123;
//string interval = "daily";
//var response = connect.historicalData(exchange,scripCode,interval);
//var responseObject = JsonConvert.DeserializeObject(response);
//var responseString = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
//Console.WriteLine(responseString);


//        }
//    }
//}
