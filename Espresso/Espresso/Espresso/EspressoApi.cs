using Espresso.Espresso.Espresso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using static System.Net.WebRequestMethods;
using System.IO;
using System.Xml.Linq;

namespace Espresso
{
    public class EspressoApi
    {
        protected string APIURL = "https://api.myespresso.com/espressoapi/services"; //prod endpoint

        EspressoToken Token { get; set; }

        public EspressoApi(string _accessToken = "", string _apiKey = "", string _vendorKey = "")
        {
            this.Token = new EspressoToken();
            this.Token.accessToken = _accessToken;
            this.Token.apiKey = _apiKey;
            this.Token.vendorKey = _vendorKey;
        }

        /* Validate Token data internally */
        private bool ValidateToken(EspressoToken token)
        {
            bool result = false;
            if (token != null)
            {
                if (token.accessToken != "" && token.apiKey != "")
                {
                    result = true;
                }
            }
            else
                result = false;

            return result;
        }

        public string GetLoginUrl(string apiKey, string vendorKey, long state)
        {

            string URL = "https://api.myespresso.com/espressoapi/auth/login.html?" + "api_key=" + apiKey;

            if (vendorKey != null)
            {
                URL += "&vendor_key=" + vendorKey;
            }
            else
            {
                Console.WriteLine("no vendor key");
            }
            if (state != null)
            {
                URL += "&state=" + state.ToString();
            }
            else
            {
                Console.WriteLine("no state");
            }

            return URL;

        }

        public string DecryptStringFromAES(string encryptedText, string key)
        {
            string iv = "AAAAAAAAAAAAAAAAAAAAAA==";
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Convert.FromBase64String(iv);

            string base64EncryptedText = encryptedText.Replace('-', '+').Replace('_', '/').PadRight(encryptedText.Length + (4 - encryptedText.Length % 4) % 4, '=');

            byte[] encryptedBytes = Convert.FromBase64String(base64EncryptedText);

            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
            AeadParameters parameters = new AeadParameters(new KeyParameter(keyBytes), 128, ivBytes, null);
            cipher.Init(false, parameters);

            byte[] decryptedBytes = new byte[cipher.GetOutputSize(encryptedBytes.Length)];
            int len = cipher.ProcessBytes(encryptedBytes, 0, encryptedBytes.Length, decryptedBytes, 0);
            cipher.DoFinal(decryptedBytes, len);

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public string EncryptStringToAES(string plaintext, string key)
        {
            string iv = "AAAAAAAAAAAAAAAAAAAAAA==";
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Convert.FromBase64String(iv);
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
            AeadParameters parameters = new AeadParameters(new KeyParameter(keyBytes), 128, ivBytes, null);
            cipher.Init(true, parameters);

            byte[] encryptedBytes = new byte[cipher.GetOutputSize(plaintextBytes.Length)];
            int len = cipher.ProcessBytes(plaintextBytes, 0, plaintextBytes.Length, encryptedBytes, 0);
            cipher.DoFinal(encryptedBytes, len);

            return Convert.ToBase64String(encryptedBytes);
        }

        public string GenerateAccessToken(string api_Key, string request_Token, int state01, string secretKey, string vendorKey)
        {
            try
            {
                string URL = APIURL + "/access/token";
                Console.WriteLine(URL);
                string token = request_Token;
                string key = secretKey;
                string decData = DecryptStringFromAES(token, key);
                string[] tokenParts = decData.Split('|');

                string newToken = tokenParts[1] + "|" + tokenParts[0];
                string encData = EncryptStringToAES(newToken, key);
                string formattedEncData = encData.Replace('+', '-').Replace('/', '_');

                Console.WriteLine("Encrypted token: " + formattedEncData);

                var requestData = new
                {
                    apiKey = api_Key,
                    requestToken = formattedEncData,
                    vendorKey = vendorKey,
                    state = state01


                };
                var json = JsonConvert.SerializeObject(requestData);

                string Json = POSTWebRequest0(URL, json);

                return Json;


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string POSTWebRequest0(string URL, string Data)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);

                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";

                byte[] byteArray = Encoding.UTF8.GetBytes(Data);
                httpWebRequest.ContentLength = byteArray.Length;
                string Json = "";

                using (Stream dataStream = httpWebRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                using (WebResponse response = httpWebRequest.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        Json = reader.ReadToEnd();
                    }
                }

                return Json;
            }
            catch (Exception ex)
            {
                return "PostError:" + ex.Message;
            }
        }




        private string POSTWebRequest(EspressoToken agr, string URL, string Data)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                if (agr != null)
                {
                    // Add the access token and api key to the headers
                    httpWebRequest.Headers.Add("access-token", agr.accessToken);
                    httpWebRequest.Headers.Add("api-key", agr.apiKey);

                    if (!string.IsNullOrEmpty(agr.vendorKey))
                    {
                        httpWebRequest.Headers.Add("vendor-key", agr.vendorKey);
                    }
                }


                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";

                string nonNullData = RemoveNullProperties(Data);
                byte[] byteArray = Encoding.UTF8.GetBytes(nonNullData);

                httpWebRequest.ContentLength = byteArray.Length;
                string Json = "";



                using (Stream dataStream = httpWebRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);


                }

                using (WebResponse response = httpWebRequest.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        Json = reader.ReadToEnd();
                    }
                }

                return Json;
            }
            catch (Exception ex)
            {
                return "PostError:" + ex.Message;
            }
        }
        private string RemoveNullProperties(string json)
        {
            var jsonObject = JObject.Parse(json);
            var properties = jsonObject.Properties().ToList();

            foreach (var property in properties)
            {
                if (property.Value.Type == JTokenType.Null)
                {
                    property.Remove();
                }
            }

            return jsonObject.ToString();
        }

        private string GETWebRequest(EspressoToken agr, string URL)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                if (agr != null)
                {
                    // Add the access token and api key to the headers
                    httpWebRequest.Headers.Add("access-token", agr.accessToken);
                    httpWebRequest.Headers.Add("api-key", agr.apiKey);

                    if (!string.IsNullOrEmpty(agr.vendorKey))
                    {
                        httpWebRequest.Headers.Add("vendor-key", agr.vendorKey);
                    }
                }

                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";

                string Json = "";
                WebResponse response = httpWebRequest.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    Json = reader.ReadToEnd();
                }
                return Json;
            }
            catch (Exception ex)
            {
                return "GetError:" + ex.Message;
            }
        }

        private string GETWebRequest0(EspressoToken agr, string URL)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                if (agr != null)
                {
                    // Add the access token and api key to the headers

                    httpWebRequest.Headers.Add("api-key", agr.apiKey);

                    if (!string.IsNullOrEmpty(agr.vendorKey))
                    {
                        httpWebRequest.Headers.Add("vendor-key", agr.vendorKey);
                    }
                }

                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "text/xml"; // Update content type
                httpWebRequest.Accept = "text/xml"; // Update accept type

                string Json = "";
                WebResponse response = httpWebRequest.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    Json = reader.ReadToEnd();
                }
                return Json;
            }
            catch (Exception ex)
            {
                return "GetError:" + ex.Message;
            }
        }




        public string placeOrder(OrderInfo order)
        {
            try
            {
                EspressoToken Token = this.Token;

                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/orders";
                        Console.WriteLine(URL);

                        string PostData = JsonConvert.SerializeObject(order);

                        string Json = POSTWebRequest(Token, URL, PostData);

                        return Json;
                    }
                    else
                    {
                        return "The token is invalid";
                    }
                }
                else
                {
                    return "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string modifyOrder(OrderInfo order)
        {
            try
            {
                EspressoToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/orders";

                        string PostData = JsonConvert.SerializeObject(order);
                        //Console.WriteLine(PostData);
                        //Console.WriteLine(URL);
                        //Console.WriteLine(Token);

                        string Json = POSTWebRequest(Token, URL, PostData);


                        return Json;
                    }
                    else
                    {
                        return "The token is invalid";
                    }
                }
                else
                {
                    return "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string cancelOrder(OrderInfo order)
        {
            try
            {
                EspressoToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/orders";

                        string PostData = JsonConvert.SerializeObject(order);

                        string Json = POSTWebRequest(Token, URL, PostData);

                        return Json;
                    }
                    else
                    {
                        return "The token is invalid";
                    }
                }
                else
                {
                    return "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string funds(string exchange, int customerId)
        {
            try
            {
                EspressoToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/limitstmt/" + exchange + "/" + customerId;

                        string Json = GETWebRequest(Token, URL);

                        return Json;
                    }
                    else
                    {
                        return "The token is invalid";
                    }
                }
                else
                {
                    return "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public string cancelByOrderId(int orderId)
        {
            try
            {
                EspressoToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/cancelOrder/" + orderId;



                        string Json = GETWebRequest(Token, URL);

                        return Json;
                    }
                    else
                    {
                        return "The token is invalid";
                    }
                }
                else
                {
                    return "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string orders(int customerId)
        {
            try
            {
                EspressoToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/reports/" + customerId;



                        string Json = GETWebRequest(Token, URL);

                        return Json;
                    }
                    else
                    {
                        return "The token is invalid";
                    }
                }
                else
                {
                    return "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string positions(int customerId)
        {
            try
            {
                EspressoToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/trades/" + customerId;



                        string Json = GETWebRequest(Token, URL);

                        return Json;
                    }
                    else
                    {
                        return "The token is invalid";
                    }
                }
                else
                {
                    return "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string history(string exchange, int customerId, int orderId)
        {
            try
            {
                EspressoToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/reports/" + exchange + "/" + customerId + "/" + orderId;



                        string Json = GETWebRequest(Token, URL);

                        return Json;
                    }
                    else
                    {
                        return "The token is invalid";
                    }
                }
                else
                {
                    return "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string trade(string exchange, int customerId, int orderId)
        {
            try
            {
                EspressoToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/reports/" + exchange + "/" + customerId + "/" + orderId + "/trades";



                        string Json = GETWebRequest(Token, URL);

                        return Json;
                    }
                    else
                    {
                        return "The token is invalid";
                    }
                }
                else
                {
                    return "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string holdings(int customerId)
        {
            try
            {
                EspressoToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/holdings/" + customerId;



                        string Json = GETWebRequest(Token, URL);

                        return Json;
                    }
                    else
                    {
                        return "The token is invalid";
                    }
                }
                else
                {
                    return "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string activeScrips(string exchange)
        {
            try
            {
                EspressoToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/master/" + exchange;



                        string Json = GETWebRequest(Token, URL);

                        return Json;
                    }
                    else
                    {
                        return "The token is invalid";
                    }
                }
                else
                {
                    return "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public string symbol(string exchange)
        {
            try
            {
                EspressoToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/master/csv" + exchange;



                        string Json = GETWebRequest0(Token, URL);

                        return Json;
                    }
                    else
                    {
                        return "The token is invalid";
                    }
                }
                else
                {
                    return "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string historicalData(string exchange, int scripCode, string interval)
        {
            try
            {
                EspressoToken Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = APIURL + "/historical/" + exchange + "/" + scripCode + "/" + interval;



                        string Json = GETWebRequest(Token, URL);

                        return Json;
                    }
                    else
                    {
                        return "The token is invalid";
                    }
                }
                else
                {
                    return "The token is invalid";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
