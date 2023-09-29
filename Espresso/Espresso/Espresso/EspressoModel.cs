using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Espresso.Espresso.Espresso
{
    class EspressoModel
    {
    }
    public class EspressoToken
    {
        public string accessToken { get; set; }
        public string apiKey { get; set; }
        public string vendorKey { get; set; }

    }
    public class OrderInfo
    {
        public string orderId { get; set; }
        public int customerId { get; set; }
        public int scripCode { get; set; }
        public string tradingSymbol { get; set; }
        public string exchange { get; set; }
        public string transactionType { get; set; }
        public int quantity { get; set; }
        public int disclosedQty { get; set; }
        public int executedQty { get; set; }
        public string price { get; set; }
        public string triggerPrice { get; set; }
        public string rmsCode { get; set; }
        public string afterHour { get; set; }
        public string orderType { get; set; }
        public string channelUser { get; set; }
        public string validity { get; set; }
        public string requestType { get; set; }
        public string productType { get; set; }

        public string instrumentType { get; set; }
        public string strikePrice { get; set; }
        public string optionType { get; set; }
        public string expiry { get; set; }
    }


}
